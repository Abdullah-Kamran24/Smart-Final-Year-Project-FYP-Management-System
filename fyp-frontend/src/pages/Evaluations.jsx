import React, { useEffect, useState } from 'react';
import api from '../api';
import { useAuth } from '../context/AuthContext';

const calcGrade = (m) => m >= 90 ? 'A+' : m >= 80 ? 'A' : m >= 70 ? 'B' : m >= 60 ? 'C' : m >= 50 ? 'D' : 'F';
const gradeColor = (g) => ({ 'A+': '#7c5cfc', A: '#39d98a', B: '#00e5ff', C: '#ffc800', D: '#ff9f43', F: '#ff6b6b' }[g] || '#888');

export default function Evaluations() {
  const { user } = useAuth();
  const [evals, setEvals] = useState([]);
  const [projects, setProjects] = useState([]);
  const [loading, setLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [editEval, setEditEval] = useState(null);
  const [form, setForm] = useState({ projectId: '', marks: '', feedback: '' });
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  // Live grade preview
  const previewGrade = form.marks !== '' ? calcGrade(Number(form.marks)) : null;

  const isSupervisor = user?.role === 'Supervisor';

  const load = async () => {
    setLoading(true);
    try {
      const [evalRes, projRes] = await Promise.all([
        api.get('/evaluation'),
        api.get('/project'),
      ]);
      setEvals(evalRes.data);
      setProjects(projRes.data);
    } catch {}
    setLoading(false);
  };

  useEffect(() => { load(); }, []);

  const openCreate = () => { setEditEval(null); setForm({ projectId: '', marks: '', feedback: '' }); setError(''); setShowModal(true); };
  const openEdit   = (e) => { setEditEval(e); setForm({ projectId: e.projectId, marks: e.marks, feedback: e.feedback || '' }); setError(''); setShowModal(true); };
  const closeModal = ()  => { setShowModal(false); setError(''); };

  const handleSubmit = async (ev) => {
    ev.preventDefault(); setError('');
    const marks = Number(form.marks);
    if (marks < 0 || marks > 100) return setError('Marks must be 0–100.');
    try {
      if (editEval) {
        await api.put(`/evaluation/${editEval.id}`, { projectId: Number(form.projectId), marks, feedback: form.feedback });
        setSuccess('Evaluation updated!');
      } else {
        await api.post('/evaluation', { projectId: Number(form.projectId), marks, feedback: form.feedback });
        setSuccess('Evaluation submitted!');
      }
      closeModal(); load(); setTimeout(() => setSuccess(''), 3000);
    } catch (err) { setError(err.response?.data?.message || 'Failed.'); }
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Delete this evaluation?')) return;
    await api.delete(`/evaluation/${id}`);
    setSuccess('Deleted.'); load(); setTimeout(() => setSuccess(''), 3000);
  };

  return (
    <div>
      <div className="section-header">
        <h1 className="page-title" style={{ marginBottom: 0 }}>Evaluations</h1>
        {isSupervisor && <button className="btn btn-primary" onClick={openCreate}>+ Add Evaluation</button>}
      </div>

      {error   && <div className="alert alert-error">{error}</div>}
      {success && <div className="alert alert-success">{success}</div>}

      {/* Grade legend */}
      <div className="card" style={{ marginBottom: '20px' }}>
        <div style={{ display: 'flex', gap: '16px', flexWrap: 'wrap', alignItems: 'center' }}>
          <span style={{ fontSize: '11px', color: 'var(--text-muted)', letterSpacing: '1px', textTransform: 'uppercase' }}>Grade Scale:</span>
          {[['A+ (90–100)', '#7c5cfc'], ['A (80–89)', '#39d98a'], ['B (70–79)', '#00e5ff'], ['C (60–69)', '#ffc800'], ['D (50–59)', '#ff9f43'], ['F (<50)', '#ff6b6b']].map(([label, color]) => (
            <span key={label} style={{ fontSize: '11px', color, fontWeight: 700 }}>{label}</span>
          ))}
        </div>
      </div>

      {loading ? (
        <div style={{ color: 'var(--text-muted)' }}>Loading...</div>
      ) : evals.length === 0 ? (
        <div className="card" style={{ textAlign: 'center', color: 'var(--text-muted)', padding: '60px' }}>No evaluations yet.</div>
      ) : (
        <div className="table-container">
          <table>
            <thead>
              <tr>
                <th>#</th>
                <th>Project</th>
                <th>Student</th>
                <th>Marks</th>
                <th>Grade</th>
                <th>Feedback</th>
                <th>Date</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {evals.map(e => {
                const g = e.grade || calcGrade(e.marks);
                return (
                  <tr key={e.id}>
                    <td style={{ color: 'var(--text-muted)' }}>{e.id}</td>
                    <td><strong>{e.projectTitle || '—'}</strong></td>
                    <td>{e.studentName || '—'}</td>
                    <td>
                      <div style={{ display: 'flex', alignItems: 'center', gap: '8px' }}>
                        <span style={{ color: 'var(--text-primary)', fontWeight: 700 }}>{e.marks}</span>
                        <div style={{ width: '60px', height: '5px', background: 'var(--bg-secondary)', borderRadius: '3px', overflow: 'hidden' }}>
                          <div style={{ width: `${e.marks}%`, height: '100%', background: gradeColor(g), borderRadius: '3px' }} />
                        </div>
                      </div>
                    </td>
                    <td>
                      <span style={{ color: gradeColor(g), fontFamily: 'var(--font-display)', fontWeight: 800, fontSize: '16px' }}>{g}</span>
                    </td>
                    <td style={{ maxWidth: '200px', fontSize: '12px', color: 'var(--text-secondary)' }}>
                      <div style={{ overflow: 'hidden', textOverflow: 'ellipsis', whiteSpace: 'nowrap' }}>{e.feedback || '—'}</div>
                    </td>
                    <td style={{ color: 'var(--text-muted)', fontSize: '12px' }}>{new Date(e.evaluatedAt).toLocaleDateString()}</td>
                    <td>
                      {isSupervisor && (
                        <div style={{ display: 'flex', gap: '6px' }}>
                          <button className="btn btn-secondary btn-sm" onClick={() => openEdit(e)}>Edit</button>
                          <button className="btn btn-danger btn-sm" onClick={() => handleDelete(e.id)}>Del</button>
                        </div>
                      )}
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>
      )}

      {/* Modal */}
      {showModal && (
        <div className="modal-overlay" onClick={e => e.target === e.currentTarget && closeModal()}>
          <div className="modal">
            <div className="modal-title">{editEval ? 'Edit Evaluation' : 'New Evaluation'}</div>
            {error && <div className="alert alert-error">{error}</div>}
            <form onSubmit={handleSubmit}>
              {!editEval && (
                <div className="form-group">
                  <label className="form-label">Project</label>
                  <select className="form-control" value={form.projectId} onChange={e => setForm({ ...form, projectId: e.target.value })} required>
                    <option value="">— Select Project —</option>
                    {projects.map(p => <option key={p.id} value={p.id}>{p.title}</option>)}
                  </select>
                </div>
              )}
              <div className="form-group">
                <label className="form-label">
                  Marks (0–100)
                  {previewGrade && (
                    <span style={{ marginLeft: '10px', color: gradeColor(previewGrade), fontFamily: 'var(--font-display)', fontWeight: 800 }}>
                      → Grade: {previewGrade}
                    </span>
                  )}
                </label>
                <input type="number" min="0" max="100" className="form-control" value={form.marks} onChange={e => setForm({ ...form, marks: e.target.value })} required />
                {/* Visual bar */}
                {form.marks !== '' && (
                  <div style={{ marginTop: '8px', height: '6px', background: 'var(--bg-secondary)', borderRadius: '3px', overflow: 'hidden' }}>
                    <div style={{ width: `${Math.min(Number(form.marks), 100)}%`, height: '100%', background: gradeColor(previewGrade), borderRadius: '3px', transition: 'width 0.3s ease' }} />
                  </div>
                )}
              </div>
              <div className="form-group">
                <label className="form-label">Feedback</label>
                <textarea className="form-control" value={form.feedback} onChange={e => setForm({ ...form, feedback: e.target.value })} placeholder="Write feedback for the student..." />
              </div>
              <div className="modal-actions">
                <button type="button" className="btn btn-secondary" onClick={closeModal}>Cancel</button>
                <button type="submit" className="btn btn-primary">{editEval ? 'Update' : 'Submit'}</button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
