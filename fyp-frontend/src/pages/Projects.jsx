import { useEffect, useState } from 'react'
import api from '../api'
import { useAuth } from '../context/AuthContext'

const statusBadge = s => {
  const m = { Active: 'badge-green', Pending: 'badge-yellow', Completed: 'badge-blue', Rejected: 'badge-red' }
  return <span className={`badge ${m[s] || 'badge-gray'}`}>{s}</span>
}

export default function Projects() {
  const { user } = useAuth()
  const [projects, setProjects]   = useState([])
  const [groups, setGroups]       = useState([])
  const [allGroups, setAllGroups] = useState([])
  const [loading, setLoading]     = useState(true)
  const [showModal, setShowModal] = useState(false)
  const [editProject, setEditProject] = useState(null)
  const [msg, setMsg]             = useState({ type: '', text: '' })
  const [form, setForm]           = useState({ title: '', description: '', technologyStack: '', groupId: '' })
  const [assigning, setAssigning] = useState(null)

  const load = async () => {
    try {
      const [pRes, gRes] = await Promise.all([
        user.role === 'Student'
          ? api.get(`/project/student/${user.userId}`)
          : user.role === 'Supervisor'
            ? api.get(`/project/supervisor/${user.userId}`)
            : api.get('/project'),
        api.get('/group')
      ])
      setProjects(pRes.data)
      setAllGroups(gRes.data)
      // Only groups without a project — for create form
      setGroups(gRes.data.filter(g => !g.projectTitle))
    } catch {}
    setLoading(false)
  }

  useEffect(() => { load() }, [])

  const flash = (type, text) => {
    setMsg({ type, text })
    setTimeout(() => setMsg({ type: '', text: '' }), 3000)
  }

  const openCreate = () => {
    setEditProject(null)
    setForm({ title: '', description: '', technologyStack: '', groupId: '' })
    setShowModal(true)
  }

  const openEdit = project => {
    setEditProject(project)
    setForm({
      title: project.title || '',
      description: project.description || '',
      technologyStack: project.technologyStack || '',
      groupId: String(project.groupId)
    })
    setShowModal(true)
  }

  const closeModal = () => {
    setShowModal(false)
    setEditProject(null)
    setForm({ title: '', description: '', technologyStack: '', groupId: '' })
  }

  const handleSubmit = async e => {
    e.preventDefault()
    try {
      const payload = {
        title:           form.title,
        description:     form.description,
        technologyStack: form.technologyStack,
        groupId:         parseInt(form.groupId)
      }
      if (editProject) {
        await api.put(`/project/${editProject.id}`, payload)
        flash('success', 'Project updated successfully!')
      } else {
        await api.post('/project', payload)
        flash('success', 'Project created successfully!')
      }
      closeModal()
      load()
    } catch (err) {
      flash('error', err.response?.data?.message || 'Failed to save project.')
    }
  }

  const handleDelete = async id => {
    if (!confirm('Delete this project?')) return
    try {
      await api.delete(`/project/${id}`)
      flash('success', 'Project deleted.')
      load()
    } catch { flash('error', 'Failed to delete project.') }
  }

  const handleAiAssign = async id => {
    setAssigning(id)
    try {
      const res = await api.post(`/project/ai-assign/${id}`)
      flash('success', res.data.message)
      load()
    } catch (err) {
      flash('error', err.response?.data?.message || 'Assignment failed.')
    } finally { setAssigning(null) }
  }

  if (loading) return <div style={{ color: 'var(--text-muted)', padding: '2rem' }}>Loading projects…</div>

  return (
    <div className="fade-in">
      <div className="page-header" style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', flexWrap: 'wrap', gap: '1rem' }}>
        <div>
          <h1 className="page-title">📁 Projects</h1>
          <p className="page-sub">{projects.length} project{projects.length !== 1 ? 's' : ''} found</p>
        </div>
        {user.role === 'Admin' && (
          <button className="btn btn-primary" onClick={openCreate}>+ New Project</button>
        )}
      </div>

      {msg.text && <div className={`alert alert-${msg.type}`}>{msg.text}</div>}

      <div className="card">
        <div className="table-wrap">
          <table>
            <thead>
              <tr>
                <th>#</th>
                <th>Title</th>
                <th>Tech Stack</th>
                <th>Group</th>
                <th>Members</th>
                <th>Supervisor</th>
                <th>Status</th>
                {user.role === 'Admin' && <th>Actions</th>}
              </tr>
            </thead>
            <tbody>
              {projects.length === 0 && (
                <tr>
                  <td colSpan={8} style={{ textAlign: 'center', color: 'var(--text-muted)', padding: '2rem' }}>
                    No projects found.
                  </td>
                </tr>
              )}
              {projects.map((p, i) => (
                <tr key={p.id}>
                  <td style={{ color: 'var(--text-muted)', fontFamily: 'var(--mono)' }}>{i + 1}</td>
                  <td style={{ fontWeight: 600, maxWidth: 200 }}>{p.title}</td>
                  <td>
                    <span style={{ fontFamily: 'var(--mono)', fontSize: '0.78rem', color: '#6366f1' }}>
                      {p.technologyStack || '—'}
                    </span>
                  </td>
                  <td>
                    <span style={{ fontWeight: 600, fontSize: '0.85rem' }}>
                      {p.groupName || '—'}
                    </span>
                  </td>
                  <td style={{ maxWidth: 180 }}>
                    <span style={{ fontSize: '0.78rem', color: 'var(--text-muted)' }}>
                      {p.memberNames || '—'}
                    </span>
                  </td>
                  <td>
                    {p.supervisorName
                      ? <span style={{ color: 'var(--green)', fontSize: '0.85rem' }}>👨‍🏫 {p.supervisorName}</span>
                      : <span style={{ color: 'var(--text-muted)', fontSize: '0.85rem' }}>Not Assigned</span>
                    }
                  </td>
                  <td>{statusBadge(p.status)}</td>
                  {user.role === 'Admin' && (
                    <td>
                      <div style={{ display: 'flex', gap: '0.4rem', flexWrap: 'wrap' }}>
                        {!p.supervisorId && (
                          <button
                            className="btn btn-success btn-sm"
                            onClick={() => handleAiAssign(p.id)}
                            disabled={assigning === p.id}
                          >
                            {assigning === p.id ? '…' : '🤖 AI'}
                          </button>
                        )}
                        <button
                          className="btn btn-secondary btn-sm"
                          onClick={() => openEdit(p)}
                        >
                          Edit
                        </button>
                        <button
                          className="btn btn-danger btn-sm"
                          onClick={() => handleDelete(p.id)}
                        >
                          🗑
                        </button>
                      </div>
                    </td>
                  )}
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>

      {/* Create Modal */}
      {showModal && (
        <div className="modal-overlay" onClick={e => e.target === e.currentTarget && closeModal()}>
          <div className="modal-box">
            <h2 className="modal-title">{editProject ? 'Edit Project' : '📁 Create New Project'}</h2>
            <form onSubmit={handleSubmit}>
              <div className="form-group">
                <label className="form-label">Project Title</label>
                <input
                  placeholder="e.g. Smart FYP Management System"
                  value={form.title}
                  onChange={e => setForm({...form, title: e.target.value})}
                  required
                />
              </div>
              <div className="form-group">
                <label className="form-label">Description</label>
                <textarea
                  rows={3}
                  placeholder="Describe the project…"
                  value={form.description}
                  onChange={e => setForm({...form, description: e.target.value})}
                />
              </div>
              <div className="form-group">
                <label className="form-label">Technology Stack</label>
                <input
                  placeholder="e.g. React, ASP.NET Core, SQL Server"
                  value={form.technologyStack}
                  onChange={e => setForm({...form, technologyStack: e.target.value})}
                />
              </div>
              <div className="form-group">
                <label className="form-label">Assign to Group</label>
                <select
                  value={form.groupId}
                  onChange={e => setForm({...form, groupId: e.target.value})}
                  required
                >
                  <option value="">Select a group without a project…</option>
                  {(editProject ? allGroups : groups).map(g => (
                    <option key={g.id} value={g.id}>
                      {g.groupName} ({g.members?.length ?? 0} members)
                    </option>
                  ))}
                </select>
              </div>
              <div style={{ display: 'flex', gap: '0.75rem', marginTop: '1.5rem' }}>
                <button type="submit" className="btn btn-primary" style={{ flex: 1, justifyContent: 'center' }}>
                  {editProject ? 'Update Project' : 'Create Project'}
                </button>
                <button type="button" className="btn btn-outline" onClick={closeModal}>
                  Cancel
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  )
}
