import React, { useEffect, useState } from 'react';
import api from '../api';
import { useAuth } from '../context/AuthContext';

export default function Proposals() {
  const { user } = useAuth();
  const [proposals, setProposals] = useState([]);
  const [projects, setProjects] = useState([]);
  const [loading, setLoading] = useState(true);
  const [selectedProjectId, setSelectedProjectId] = useState('');
  const [reviewModal, setReviewModal] = useState(null); // proposal being reviewed
  const [reviewForm, setReviewForm] = useState({ status: 'Approved', remarks: '' });
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  const isSupervisor = user?.role === 'Supervisor';

  const load = async () => {
    setLoading(true);
    try {
      const [propRes, projRes] = await Promise.all([
        api.get('/proposal'),
        api.get(user?.role === 'Student' ? `/project/student/${user.userId}` : '/project'),
      ]);
      setProposals(propRes.data);
      setProjects(projRes.data);
    } catch {}
    setLoading(false);
  };

  useEffect(() => { load(); }, []);

  const handleSubmit = async (e) => {
    e.preventDefault(); setError(''); setSuccess('');
    if (!selectedProjectId) return setError('Select a project first.');
    try {
      await api.post('/proposal', { projectId: Number(selectedProjectId) });
      setSuccess('Proposal submitted successfully!');
      setSelectedProjectId('');
      load(); setTimeout(() => setSuccess(''), 3000);
    } catch (err) { setError(err.response?.data?.message || 'Failed to submit.'); }
  };

  const handleReview = async (e) => {
    e.preventDefault(); setError('');
    try {
      await api.put(`/proposal/${reviewModal.id}`, reviewForm);
      setSuccess(`Proposal ${reviewForm.status}!`);
      setReviewModal(null);
      load(); setTimeout(() => setSuccess(''), 3000);
    } catch (err) { setError(err.response?.data?.message || 'Update failed.'); }
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Delete this proposal?')) return;
    await api.delete(`/proposal/${id}`);
    load();
  };

  const getBadge = (s) => {
    const map = { Pending: 'badge-pending', Approved: 'badge-approved', Rejected: 'badge-rejected' };
    return `badge ${map[s] || 'badge-pending'}`;
  };

  // Projects without existing proposals (for student submission)
  const availableProjects = projects.filter(p => !proposals.find(pr => pr.projectId === p.id));

  return (
    <div>
      <h1 className="page-title">Proposals</h1>

      {error   && <div className="alert alert-error">{error}</div>}
      {success && <div className="alert alert-success">{success}</div>}

      {/* Submit section for students */}
      {!isSupervisor && (
        <div className="card" style={{ marginBottom: '24px' }}>
          <div className="section-title" style={{ marginBottom: '16px' }}>Submit Proposal</div>
          <form onSubmit={handleSubmit} style={{ display: 'flex', gap: '12px', flexWrap: 'wrap', alignItems: 'flex-end' }}>
            <div className="form-group" style={{ flex: 1, minWidth: '200px', marginBottom: 0 }}>
              <label className="form-label">Select Project</label>
              <select className="form-control" value={selectedProjectId} onChange={e => setSelectedProjectId(e.target.value)}>
                <option value="">— Choose a project —</option>
                {availableProjects.map(p => <option key={p.id} value={p.id}>{p.title}</option>)}
              </select>
            </div>
            <button type="submit" className="btn btn-primary">Submit Proposal</button>
          </form>
          {availableProjects.length === 0 && (
            <div style={{ marginTop: '12px', color: 'var(--text-muted)', fontSize: '12px' }}>
              All your projects already have proposals, or you have no projects yet.
            </div>
          )}
        </div>
      )}

      {/* Proposals table */}
      {loading ? (
        <div style={{ color: 'var(--text-muted)' }}>Loading...</div>
      ) : proposals.length === 0 ? (
        <div className="card" style={{ textAlign: 'center', color: 'var(--text-muted)', padding: '60px' }}>No proposals yet.</div>
      ) : (
        <div className="table-container">
          <table>
            <thead>
              <tr>
                <th>#</th>
                <th>Project</th>
                <th>Student</th>
                <th>Status</th>
                <th>Remarks</th>
                <th>Submitted</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {proposals.map(p => (
                <tr key={p.id}>
                  <td style={{ color: 'var(--text-muted)' }}>{p.id}</td>
                  <td><strong>{p.projectTitle || '—'}</strong></td>
                  <td>{p.studentName || '—'}</td>
                  <td><span className={getBadge(p.status)}>{p.status}</span></td>
                  <td style={{ color: 'var(--text-muted)', fontSize: '12px' }}>{p.remarks || '—'}</td>
                  <td style={{ color: 'var(--text-muted)', fontSize: '12px' }}>{new Date(p.submittedAt).toLocaleDateString()}</td>
                  <td>
                    <div style={{ display: 'flex', gap: '6px' }}>
                      {isSupervisor && p.status === 'Pending' && (
                        <button className="btn btn-secondary btn-sm" onClick={() => { setReviewModal(p); setReviewForm({ status: 'Approved', remarks: '' }); }}>
                          Review
                        </button>
                      )}
                      {isSupervisor && (
                        <button className="btn btn-danger btn-sm" onClick={() => handleDelete(p.id)}>Del</button>
                      )}
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {/* Review modal */}
      {reviewModal && (
        <div className="modal-overlay" onClick={e => e.target === e.currentTarget && setReviewModal(null)}>
          <div className="modal">
            <div className="modal-title">Review Proposal</div>
            <div style={{ color: 'var(--text-muted)', marginBottom: '16px', fontSize: '13px' }}>
              Project: <strong style={{ color: 'var(--text-primary)' }}>{reviewModal.projectTitle}</strong>
            </div>
            <form onSubmit={handleReview}>
              <div className="form-group">
                <label className="form-label">Decision</label>
                <select className="form-control" value={reviewForm.status} onChange={e => setReviewForm({ ...reviewForm, status: e.target.value })}>
                  <option value="Approved">Approve</option>
                  <option value="Rejected">Reject</option>
                </select>
              </div>
              <div className="form-group">
                <label className="form-label">Remarks (optional)</label>
                <textarea className="form-control" value={reviewForm.remarks} onChange={e => setReviewForm({ ...reviewForm, remarks: e.target.value })} placeholder="Add feedback for the student..." />
              </div>
              <div className="modal-actions">
                <button type="button" className="btn btn-secondary" onClick={() => setReviewModal(null)}>Cancel</button>
                <button type="submit" className={`btn ${reviewForm.status === 'Approved' ? 'btn-success' : 'btn-danger'}`}>
                  {reviewForm.status === 'Approved' ? '✓ Approve' : '✗ Reject'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
