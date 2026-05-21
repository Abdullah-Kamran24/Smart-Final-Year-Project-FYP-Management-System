import { useEffect, useState } from 'react'
import api from '../api'
import { useAuth } from '../context/AuthContext'

const emptyForm = {
  projectId: '',
  title: '',
  type: 'Milestone',
  status: 'Pending',
  dueDate: '',
  description: ''
}

const badgeClass = status => {
  const s = (status || '').toLowerCase().replaceAll(' ', '-')
  return `badge badge-${s}`
}

const toFormDate = value => value ? new Date(value).toISOString().slice(0, 10) : ''

export default function Deliverables() {
  const { user } = useAuth()
  const [deliverables, setDeliverables] = useState([])
  const [projects, setProjects] = useState([])
  const [loading, setLoading] = useState(true)
  const [showModal, setShowModal] = useState(false)
  const [editing, setEditing] = useState(null)
  const [uploadTarget, setUploadTarget] = useState(null)
  const [file, setFile] = useState(null)
  const [form, setForm] = useState(emptyForm)
  const [msg, setMsg] = useState({ type: '', text: '' })
  const [filter, setFilter] = useState('All')

  const load = async () => {
    setLoading(true)
    try {
      const [dRes, pRes] = await Promise.all([
        api.get('/deliverable'),
        api.get(user?.role === 'Student' ? `/project/student/${user.userId}` : '/project')
      ])
      setDeliverables(dRes.data)
      setProjects(pRes.data)
    } catch {
      flash('error', 'Unable to load deliverables.')
    }
    setLoading(false)
  }

  useEffect(() => { load() }, [])

  const flash = (type, text) => {
    setMsg({ type, text })
    setTimeout(() => setMsg({ type: '', text: '' }), 3000)
  }

  const openCreate = () => {
    setEditing(null)
    setForm(emptyForm)
    setShowModal(true)
  }

  const openEdit = d => {
    setEditing(d)
    setForm({
      projectId: String(d.projectId),
      title: d.title,
      type: d.type,
      status: d.status,
      dueDate: toFormDate(d.dueDate),
      description: d.description || ''
    })
    setShowModal(true)
  }

  const closeModal = () => {
    setShowModal(false)
    setEditing(null)
    setForm(emptyForm)
  }

  const handleSubmit = async e => {
    e.preventDefault()
    if (!form.projectId) return flash('error', 'Select a project.')
    if (!form.title.trim()) return flash('error', 'Title is required.')

    const payload = {
      projectId: Number(form.projectId),
      title: form.title,
      type: form.type,
      status: form.status,
      dueDate: form.dueDate || null,
      description: form.description
    }

    try {
      if (editing) {
        await api.put(`/deliverable/${editing.id}`, payload)
        flash('success', 'Deliverable updated.')
      } else {
        await api.post('/deliverable', payload)
        flash('success', 'Deliverable created.')
      }
      closeModal()
      load()
    } catch (err) {
      flash('error', err.response?.data?.message || 'Save failed.')
    }
  }

  const handleDelete = async id => {
    if (!window.confirm('Delete this deliverable?')) return
    try {
      await api.delete(`/deliverable/${id}`)
      flash('success', 'Deliverable deleted.')
      load()
    } catch {
      flash('error', 'Delete failed.')
    }
  }

  const handleUpload = async e => {
    e.preventDefault()
    if (!file) return flash('error', 'Choose a file first.')

    const fd = new FormData()
    fd.append('file', file)

    try {
      await api.post(`/deliverable/${uploadTarget.id}/upload`, fd, {
        headers: { 'Content-Type': 'multipart/form-data' }
      })
      flash('success', 'File uploaded and deliverable marked Submitted.')
      setUploadTarget(null)
      setFile(null)
      load()
    } catch (err) {
      flash('error', err.response?.data?.message || 'Upload failed.')
    }
  }

  const visible = filter === 'All'
    ? deliverables
    : deliverables.filter(d => d.type === filter)

  return (
    <div className="fade-in">
      <div className="section-header">
        <div>
          <h1 className="page-title" style={{ marginBottom: 4 }}>Deliverables</h1>
          <p style={{ color: 'var(--text-muted)' }}>Milestones, final report, and presentation tracking.</p>
        </div>
        <button className="btn btn-primary" onClick={openCreate}>+ New Deliverable</button>
      </div>

      {msg.text && <div className={`alert alert-${msg.type}`}>{msg.text}</div>}

      <div className="card" style={{ marginBottom: 20 }}>
        <div style={{ display: 'flex', gap: 10, flexWrap: 'wrap' }}>
          {['All', 'Milestone', 'Final Report', 'Presentation'].map(type => (
            <button
              key={type}
              className={`btn ${filter === type ? 'btn-primary' : 'btn-secondary'} btn-sm`}
              onClick={() => setFilter(type)}
            >
              {type}
            </button>
          ))}
        </div>
      </div>

      {loading ? (
        <div style={{ color: 'var(--text-muted)' }}>Loading...</div>
      ) : visible.length === 0 ? (
        <div className="card" style={{ textAlign: 'center', color: 'var(--text-muted)', padding: 60 }}>
          No deliverables found.
        </div>
      ) : (
        <div className="table-container">
          <table>
            <thead>
              <tr>
                <th>#</th>
                <th>Project</th>
                <th>Title</th>
                <th>Type</th>
                <th>Status</th>
                <th>Due Date</th>
                <th>File</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {visible.map(d => (
                <tr key={d.id}>
                  <td>{d.id}</td>
                  <td><strong>{d.projectTitle || '—'}</strong></td>
                  <td>
                    <div style={{ fontWeight: 700, color: 'var(--text-primary)' }}>{d.title}</div>
                    {d.description && (
                      <div style={{ fontSize: 12, color: 'var(--text-muted)', maxWidth: 260, overflow: 'hidden', textOverflow: 'ellipsis', whiteSpace: 'nowrap' }}>
                        {d.description}
                      </div>
                    )}
                  </td>
                  <td>{d.type}</td>
                  <td><span className={badgeClass(d.status)}>{d.status}</span></td>
                  <td>{d.dueDate ? new Date(d.dueDate).toLocaleDateString() : '—'}</td>
                  <td>
                    {d.filePath
                      ? <a href={d.filePath} target="_blank" rel="noreferrer" style={{ color: 'var(--accent-2)' }}>Open</a>
                      : <span style={{ color: 'var(--text-muted)' }}>—</span>}
                  </td>
                  <td>
                    <div style={{ display: 'flex', gap: 6, flexWrap: 'wrap' }}>
                      <button className="btn btn-secondary btn-sm" onClick={() => openEdit(d)}>Edit</button>
                      <button className="btn btn-success btn-sm" onClick={() => setUploadTarget(d)}>Upload</button>
                      <button className="btn btn-danger btn-sm" onClick={() => handleDelete(d.id)}>Del</button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {showModal && (
        <div className="modal-overlay" onClick={e => e.target === e.currentTarget && closeModal()}>
          <div className="modal">
            <div className="modal-title">{editing ? 'Edit Deliverable' : 'New Deliverable'}</div>
            <form onSubmit={handleSubmit}>
              <div className="form-group">
                <label className="form-label">Project</label>
                <select className="form-control" value={form.projectId} onChange={e => setForm({ ...form, projectId: e.target.value })} required>
                  <option value="">Select project</option>
                  {projects.map(p => <option key={p.id} value={p.id}>{p.title}</option>)}
                </select>
              </div>
              <div className="form-group">
                <label className="form-label">Title</label>
                <input className="form-control" value={form.title} onChange={e => setForm({ ...form, title: e.target.value })} required />
              </div>
              <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 12 }}>
                <div className="form-group">
                  <label className="form-label">Type</label>
                  <select className="form-control" value={form.type} onChange={e => setForm({ ...form, type: e.target.value })}>
                    <option>Milestone</option>
                    <option>Final Report</option>
                    <option>Presentation</option>
                  </select>
                </div>
                <div className="form-group">
                  <label className="form-label">Status</label>
                  <select className="form-control" value={form.status} onChange={e => setForm({ ...form, status: e.target.value })}>
                    <option>Pending</option>
                    <option>In Progress</option>
                    <option>Submitted</option>
                    <option>Approved</option>
                    <option>Rejected</option>
                    <option>Completed</option>
                  </select>
                </div>
              </div>
              <div className="form-group">
                <label className="form-label">Due Date</label>
                <input type="date" className="form-control" value={form.dueDate} onChange={e => setForm({ ...form, dueDate: e.target.value })} />
              </div>
              <div className="form-group">
                <label className="form-label">Description</label>
                <textarea className="form-control" value={form.description} onChange={e => setForm({ ...form, description: e.target.value })} />
              </div>
              <div className="modal-actions">
                <button type="button" className="btn btn-secondary" onClick={closeModal}>Cancel</button>
                <button type="submit" className="btn btn-primary">{editing ? 'Update' : 'Create'}</button>
              </div>
            </form>
          </div>
        </div>
      )}

      {uploadTarget && (
        <div className="modal-overlay" onClick={e => e.target === e.currentTarget && setUploadTarget(null)}>
          <div className="modal">
            <div className="modal-title">Upload File</div>
            <div style={{ color: 'var(--text-muted)', marginBottom: 16 }}>{uploadTarget.title}</div>
            <form onSubmit={handleUpload}>
              <div className="form-group">
                <label className="form-label">File</label>
                <input type="file" className="form-control" onChange={e => setFile(e.target.files?.[0] || null)} />
              </div>
              <div className="modal-actions">
                <button type="button" className="btn btn-secondary" onClick={() => setUploadTarget(null)}>Cancel</button>
                <button type="submit" className="btn btn-success">Upload</button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  )
}
