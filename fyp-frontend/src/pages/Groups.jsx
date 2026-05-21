import { useEffect, useState } from 'react'
import api from '../api'
import { useAuth } from '../context/AuthContext'

export default function Groups() {
  const { user } = useAuth()
  const [groups, setGroups]         = useState([])
  const [students, setStudents]     = useState([])
  const [loading, setLoading]       = useState(true)
  const [showModal, setShowModal]   = useState(false)
  const [detailGroup, setDetailGroup] = useState(null)
  const [editGroup, setEditGroup]   = useState(null)
  const [msg, setMsg]               = useState({ type: '', text: '' })
  const [assigning, setAssigning]   = useState(null)
  const [form, setForm]             = useState({ groupName: '', description: '', studentIds: [] })
  const [search, setSearch]         = useState('')

  const load = async () => {
    try {
      const [gRes, uRes] = await Promise.all([
        user.role === 'Supervisor'
          ? api.get(`/group/supervisor/${user.userId}`)
          : api.get('/group'),
        api.get('/auth/users')
      ])
      setGroups(gRes.data)
      setStudents(uRes.data.filter(u => u.role === 'Student'))
    } catch {}
    setLoading(false)
  }

  useEffect(() => { load() }, [])

  const flash = (type, text) => {
    setMsg({ type, text })
    setTimeout(() => setMsg({ type: '', text: '' }), 3000)
  }

  const openCreate = () => {
    setEditGroup(null)
    setForm({ groupName: '', description: '', studentIds: [] })
    setShowModal(true)
  }

  const openEdit = group => {
    setEditGroup(group)
    setForm({ groupName: group.groupName || '', description: group.description || '', studentIds: [] })
    setShowModal(true)
  }

  const closeModal = () => {
    setShowModal(false)
    setEditGroup(null)
    setForm({ groupName: '', description: '', studentIds: [] })
  }

  const handleSubmit = async e => {
    e.preventDefault()
    if (!editGroup && (form.studentIds.length < 2 || form.studentIds.length > 3)) {
      flash('error', 'Please select 2 or 3 students.')
      return
    }
    try {
      const payload = {
        groupName: form.groupName,
        description: form.description,
        studentIds: form.studentIds.map(Number)
      }
      if (editGroup) {
        await api.put(`/group/${editGroup.id}`, payload)
        flash('success', 'Group updated successfully!')
      } else {
        await api.post('/group', payload)
        flash('success', 'Group created successfully!')
      }
      closeModal()
      load()
    } catch (err) {
      flash('error', err.response?.data?.message || 'Failed to save group.')
    }
  }

  const handleDelete = async id => {
    if (!confirm('Delete this group? This will also remove the group members.')) return
    try {
      await api.delete(`/group/${id}`)
      flash('success', 'Group deleted.')
      load()
    } catch { flash('error', 'Failed to delete group.') }
  }

  const handleAiAssign = async groupId => {
    setAssigning(groupId)
    try {
      const res = await api.post(`/group/ai-assign/${groupId}`)
      flash('success', res.data.message)
      load()
    } catch (err) {
      flash('error', err.response?.data?.message || 'AI assignment failed.')
    } finally { setAssigning(null) }
  }

  const toggleStudent = id => {
    setForm(f => ({
      ...f,
      studentIds: f.studentIds.includes(id)
        ? f.studentIds.filter(s => s !== id)
        : f.studentIds.length < 3
          ? [...f.studentIds, id]
          : f.studentIds
    }))
  }

  // Students already in a group — disable them in create form
  const assignedStudentIds = groups.flatMap(g => g.members?.map(m => m.studentId) ?? [])

  const filteredGroups = groups.filter(g =>
    g.groupName.toLowerCase().includes(search.toLowerCase()) ||
    (g.projectTitle || '').toLowerCase().includes(search.toLowerCase()) ||
    (g.supervisorName || '').toLowerCase().includes(search.toLowerCase())
  )

  if (loading) return <div style={{ color: 'var(--text-muted)', padding: '2rem' }}>Loading groups…</div>

  return (
    <div className="fade-in">
      {/* Header */}
      <div className="page-header" style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', flexWrap: 'wrap', gap: '1rem' }}>
        <div>
          <h1 className="page-title">👥 Groups</h1>
          <p className="page-sub">{groups.length} group{groups.length !== 1 ? 's' : ''} · {groups.reduce((a, g) => a + (g.members?.length ?? 0), 0)} students assigned</p>
        </div>
        {user.role === 'Admin' && (
          <button className="btn btn-primary" onClick={openCreate}>+ New Group</button>
        )}
      </div>

      {msg.text && <div className={`alert alert-${msg.type}`}>{msg.text}</div>}

      {/* Search */}
      <div style={{ marginBottom: '1rem' }}>
        <input
          placeholder="🔍 Search groups, projects, supervisors…"
          value={search}
          onChange={e => setSearch(e.target.value)}
          style={{ maxWidth: 400 }}
        />
      </div>

      {/* Stats row */}
      <div className="grid-3" style={{ marginBottom: '1.5rem' }}>
        <div className="card" style={{ display: 'flex', alignItems: 'center', gap: '1rem' }}>
          <div style={{ fontSize: '2rem' }}>👥</div>
          <div>
            <div style={{ fontSize: '1.5rem', fontWeight: 800, color: 'var(--accent)' }}>{groups.length}</div>
            <div style={{ fontSize: '0.75rem', color: 'var(--text-muted)', textTransform: 'uppercase' }}>Total Groups</div>
          </div>
        </div>
        <div className="card" style={{ display: 'flex', alignItems: 'center', gap: '1rem' }}>
          <div style={{ fontSize: '2rem' }}>📁</div>
          <div>
            <div style={{ fontSize: '1.5rem', fontWeight: 800, color: 'var(--green)' }}>
              {groups.filter(g => g.projectTitle).length}
            </div>
            <div style={{ fontSize: '0.75rem', color: 'var(--text-muted)', textTransform: 'uppercase' }}>Groups with Projects</div>
          </div>
        </div>
        <div className="card" style={{ display: 'flex', alignItems: 'center', gap: '1rem' }}>
          <div style={{ fontSize: '2rem' }}>👨‍🏫</div>
          <div>
            <div style={{ fontSize: '1.5rem', fontWeight: 800, color: 'var(--yellow)' }}>
              {groups.filter(g => g.supervisorName).length}
            </div>
            <div style={{ fontSize: '0.75rem', color: 'var(--text-muted)', textTransform: 'uppercase' }}>Groups with Supervisors</div>
          </div>
        </div>
      </div>

      {/* Groups Table */}
      <div className="card">
        <div className="table-wrap">
          <table>
            <thead>
              <tr>
                <th>#</th>
                <th>Group Name</th>
                <th>Members</th>
                <th>Project</th>
                <th>Supervisor</th>
                <th>Status</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {filteredGroups.length === 0 && (
                <tr>
                  <td colSpan={7} style={{ textAlign: 'center', color: 'var(--text-muted)', padding: '2rem' }}>
                    No groups found.
                  </td>
                </tr>
              )}
              {filteredGroups.map((g, i) => (
                <tr key={g.id}>
                  <td style={{ color: 'var(--text-muted)', fontFamily: 'var(--mono)' }}>{i + 1}</td>
                  <td>
                    <div style={{ fontWeight: 600 }}>{g.groupName}</div>
                    {g.description && (
                      <div style={{ fontSize: '0.75rem', color: 'var(--text-muted)' }}>{g.description}</div>
                    )}
                  </td>
                  <td>
                    <div style={{ display: 'flex', flexDirection: 'column', gap: '0.15rem' }}>
                      {g.members?.map(m => (
                        <span key={m.studentId} style={{ fontSize: '0.8rem', color: 'var(--text-muted)' }}>
                          • {m.studentName}
                        </span>
                      ))}
                      <span style={{ fontSize: '0.7rem', color: 'var(--accent)', fontFamily: 'var(--mono)' }}>
                        {g.members?.length ?? 0}/3
                      </span>
                    </div>
                  </td>
                  <td>
                    {g.projectTitle
                      ? <span style={{ fontWeight: 600, fontSize: '0.85rem' }}>{g.projectTitle}</span>
                      : <span style={{ color: 'var(--text-muted)', fontSize: '0.85rem' }}>No project</span>
                    }
                  </td>
                  <td>
                    {g.supervisorName
                      ? <span style={{ color: 'var(--green)', fontSize: '0.85rem' }}>👨‍🏫 {g.supervisorName}</span>
                      : <span style={{ color: 'var(--text-muted)', fontSize: '0.85rem' }}>Not assigned</span>
                    }
                  </td>
                  <td>
                    {g.projectStatus
                      ? <span className={`badge ${
                          g.projectStatus === 'Active'    ? 'badge-green'  :
                          g.projectStatus === 'Completed' ? 'badge-blue'   :
                          g.projectStatus === 'Rejected'  ? 'badge-red'    : 'badge-yellow'
                        }`}>{g.projectStatus}</span>
                      : <span className="badge badge-gray">No Project</span>
                    }
                  </td>
                  <td>
                    <div style={{ display: 'flex', gap: '0.4rem', flexWrap: 'wrap' }}>
                      <button
                        className="btn btn-outline btn-sm"
                        onClick={() => setDetailGroup(g)}
                      >
                        👁 View
                      </button>
                      {user.role === 'Admin' && g.projectId && !g.supervisorName && (
                        <button
                          className="btn btn-success btn-sm"
                          onClick={() => handleAiAssign(g.id)}
                          disabled={assigning === g.id}
                        >
                          {assigning === g.id ? '…' : '🤖 AI'}
                        </button>
                      )}
                      {user.role === 'Admin' && (
                        <button
                          className="btn btn-secondary btn-sm"
                          onClick={() => openEdit(g)}
                        >
                          Edit
                        </button>
                      )}
                      {user.role === 'Admin' && (
                        <button
                          className="btn btn-danger btn-sm"
                          onClick={() => handleDelete(g.id)}
                        >
                          🗑
                        </button>
                      )}
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>

      {/* ── Detail Modal ── */}
      {detailGroup && (
        <div className="modal-overlay" onClick={e => e.target === e.currentTarget && setDetailGroup(null)}>
          <div className="modal-box" style={{ maxWidth: 560 }}>
            <h2 className="modal-title">👥 {detailGroup.groupName}</h2>

            {detailGroup.description && (
              <p style={{ color: 'var(--text-muted)', fontSize: '0.875rem', marginBottom: '1rem' }}>
                {detailGroup.description}
              </p>
            )}

            {/* Members */}
            <div style={{ marginBottom: '1.25rem' }}>
              <div style={{ fontSize: '0.75rem', fontWeight: 600, color: 'var(--text-muted)', textTransform: 'uppercase', letterSpacing: '0.05em', marginBottom: '0.5rem' }}>
                Members ({detailGroup.members?.length}/3)
              </div>
              {detailGroup.members?.map(m => (
                <div key={m.studentId} style={{
                  display: 'flex', alignItems: 'center', gap: '0.75rem',
                  padding: '0.5rem 0.75rem',
                  background: 'rgba(255,255,255,0.03)',
                  borderRadius: 8, marginBottom: '0.35rem',
                  border: '1px solid var(--border)'
                }}>
                  <div style={{
                    width: 30, height: 30,
                    background: 'rgba(59,130,246,0.15)',
                    border: '1.5px solid rgba(59,130,246,0.4)',
                    borderRadius: '50%',
                    display: 'flex', alignItems: 'center', justifyContent: 'center',
                    fontSize: '0.8rem', fontWeight: 700, color: 'var(--accent)'
                  }}>
                    {m.studentName?.[0]}
                  </div>
                  <div>
                    <div style={{ fontWeight: 600, fontSize: '0.875rem' }}>{m.studentName}</div>
                    <div style={{ fontSize: '0.75rem', color: 'var(--text-muted)' }}>{m.email}</div>
                  </div>
                </div>
              ))}
            </div>

            {/* Project info */}
            <div style={{
              padding: '0.75rem 1rem',
              background: 'rgba(59,130,246,0.06)',
              borderRadius: 8,
              border: '1px solid rgba(59,130,246,0.15)',
              marginBottom: '1rem'
            }}>
              <div style={{ fontSize: '0.75rem', fontWeight: 600, color: 'var(--text-muted)', textTransform: 'uppercase', letterSpacing: '0.05em', marginBottom: '0.4rem' }}>Project</div>
              {detailGroup.projectTitle
                ? <>
                    <div style={{ fontWeight: 700 }}>📁 {detailGroup.projectTitle}</div>
                    <div style={{ fontSize: '0.8rem', color: 'var(--text-muted)', marginTop: '0.25rem' }}>
                      Status: <span style={{ color: detailGroup.projectStatus === 'Active' ? 'var(--green)' : 'var(--yellow)' }}>{detailGroup.projectStatus}</span>
                    </div>
                  </>
                : <div style={{ color: 'var(--text-muted)', fontSize: '0.875rem' }}>No project assigned yet.</div>
              }
            </div>

            {/* Supervisor info */}
            <div style={{
              padding: '0.75rem 1rem',
              background: 'rgba(16,185,129,0.06)',
              borderRadius: 8,
              border: '1px solid rgba(16,185,129,0.15)',
              marginBottom: '1.25rem'
            }}>
              <div style={{ fontSize: '0.75rem', fontWeight: 600, color: 'var(--text-muted)', textTransform: 'uppercase', letterSpacing: '0.05em', marginBottom: '0.4rem' }}>Supervisor</div>
              {detailGroup.supervisorName
                ? <div style={{ fontWeight: 700, color: 'var(--green)' }}>👨‍🏫 {detailGroup.supervisorName}</div>
                : <div style={{ color: 'var(--text-muted)', fontSize: '0.875rem' }}>Not yet assigned.</div>
              }
            </div>

            <button className="btn btn-outline" style={{ width: '100%', justifyContent: 'center' }} onClick={() => setDetailGroup(null)}>
              Close
            </button>
          </div>
        </div>
      )}

      {/* ── Create Group Modal ── */}
      {showModal && (
        <div className="modal-overlay" onClick={e => e.target === e.currentTarget && closeModal()}>
          <div className="modal-box" style={{ maxWidth: 560 }}>
            <h2 className="modal-title">{editGroup ? 'Edit Group' : '👥 Create New Group'}</h2>
            <form onSubmit={handleSubmit}>
              <div className="form-group">
                <label className="form-label">Group Name</label>
                <input
                  placeholder="e.g. AI Innovators"
                  value={form.groupName}
                  onChange={e => setForm({...form, groupName: e.target.value})}
                  required
                />
              </div>
              <div className="form-group">
                <label className="form-label">Description (optional)</label>
                <input
                  placeholder="Brief description of the group"
                  value={form.description}
                  onChange={e => setForm({...form, description: e.target.value})}
                />
              </div>

              {!editGroup && <div className="form-group">
                <label className="form-label">
                  Select Students (2–3 required) &nbsp;
                  <span style={{ color: form.studentIds.length < 2 ? 'var(--red)' : 'var(--green)', fontFamily: 'var(--mono)' }}>
                    {form.studentIds.length}/3 selected
                  </span>
                </label>
                <input
                  placeholder="Filter students by name…"
                  style={{ marginBottom: '0.5rem' }}
                  onChange={e => {
                    const q = e.target.value.toLowerCase()
                    // just re-render filtered list via local state
                    e.target._filter = q
                    // force re-render trick
                    setForm(f => ({ ...f }))
                  }}
                  ref={el => { if (el) el._filter = el._filter || '' }}
                  id="student-filter"
                />
                <div style={{
                  maxHeight: 220,
                  overflowY: 'auto',
                  border: '1px solid var(--border)',
                  borderRadius: 8,
                  background: 'var(--bg-dark)'
                }}>
                  {students
                    .filter(s => {
                      const q = (document.getElementById('student-filter')?.value || '').toLowerCase()
                      return s.name.toLowerCase().includes(q)
                    })
                    .map(s => {
                      const taken    = assignedStudentIds.includes(s.id)
                      const selected = form.studentIds.includes(s.id)
                      return (
                        <div
                          key={s.id}
                          onClick={() => !taken && toggleStudent(s.id)}
                          style={{
                            display: 'flex',
                            alignItems: 'center',
                            gap: '0.75rem',
                            padding: '0.5rem 0.75rem',
                            cursor: taken ? 'not-allowed' : 'pointer',
                            opacity: taken ? 0.4 : 1,
                            background: selected ? 'rgba(59,130,246,0.12)' : 'transparent',
                            borderBottom: '1px solid var(--border)',
                            transition: 'background 0.15s'
                          }}
                        >
                          <div style={{
                            width: 18, height: 18,
                            borderRadius: 4,
                            border: selected ? '2px solid var(--accent)' : '2px solid var(--border)',
                            background: selected ? 'var(--accent)' : 'transparent',
                            display: 'flex', alignItems: 'center', justifyContent: 'center',
                            fontSize: '0.7rem', flexShrink: 0
                          }}>
                            {selected && '✓'}
                          </div>
                          <div>
                            <div style={{ fontSize: '0.875rem', fontWeight: selected ? 600 : 400 }}>{s.name}</div>
                            <div style={{ fontSize: '0.7rem', color: 'var(--text-muted)', fontFamily: 'var(--mono)' }}>{s.email}</div>
                          </div>
                          {taken && (
                            <span style={{ marginLeft: 'auto', fontSize: '0.7rem', color: 'var(--yellow)' }}>In group</span>
                          )}
                        </div>
                      )
                    })
                  }
                </div>
              </div>}

              <div style={{ display: 'flex', gap: '0.75rem', marginTop: '1.5rem' }}>
                <button type="submit" className="btn btn-primary" style={{ flex: 1, justifyContent: 'center' }}>
                  {editGroup ? 'Update Group' : 'Create Group'}
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
