import React, { useEffect, useState } from 'react';
import api from '../api';
import { useAuth } from '../context/AuthContext';

export default function Progress() {
  const { user } = useAuth();
  const [reports, setReports] = useState([]);
  const [projects, setProjects] = useState([]);
  const [loading, setLoading] = useState(true);
  const [form, setForm] = useState({ projectId: '', report: '' });
  const [file, setFile] = useState(null);
  const [uploading, setUploading] = useState(false);
  const [editReport, setEditReport] = useState(null);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  const load = async () => {
    setLoading(true);
    try {
      const [repRes, projRes] = await Promise.all([
        api.get('/progress'),
        api.get(user?.role === 'Student' ? `/project/student/${user.userId}` : '/project'),
      ]);
      setReports(repRes.data);
      setProjects(projRes.data);
    } catch {}
    setLoading(false);
  };

  useEffect(() => { load(); }, []);

  const handleTextSubmit = async (e) => {
    e.preventDefault(); setError(''); setSuccess('');
    if (!editReport && !form.projectId) return setError('Select a project.');
    if (!form.report.trim()) return setError('Enter a progress report.');
    try {
      if (editReport) {
        await api.put(`/progress/${editReport.id}`, { report: form.report });
        setSuccess('Progress report updated!');
      } else {
        await api.post('/progress', { projectId: Number(form.projectId), report: form.report });
        setSuccess('Progress report submitted!');
      }
      setForm({ projectId: '', report: '' });
      setEditReport(null);
      load(); setTimeout(() => setSuccess(''), 3000);
    } catch (err) { setError(err.response?.data?.message || 'Failed.'); }
  };

  const openEdit = (report) => {
    setEditReport(report);
    setForm({ projectId: String(report.projectId), report: report.report || '' });
    window.scrollTo({ top: 0, behavior: 'smooth' });
  };

  const cancelEdit = () => {
    setEditReport(null);
    setForm({ projectId: '', report: '' });
  };

  const handleFileUpload = async (e) => {
    e.preventDefault(); setError(''); setSuccess('');
    if (!form.projectId) return setError('Select a project.');
    if (!file) return setError('Select a file to upload.');
    setUploading(true);
    try {
      const fd = new FormData();
      fd.append('projectId', form.projectId);
      fd.append('file', file);
      await api.post('/progress/upload', fd, { headers: { 'Content-Type': 'multipart/form-data' } });
      setSuccess('File uploaded successfully!');
      setFile(null);
      // reset file input
      document.getElementById('file-input').value = '';
      load(); setTimeout(() => setSuccess(''), 3000);
    } catch (err) { setError(err.response?.data?.message || 'Upload failed.'); }
    setUploading(false);
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Delete this report?')) return;
    await api.delete(`/progress/${id}`);
    load();
  };

  return (
    <div>
      <h1 className="page-title">Progress Reports</h1>

      {error   && <div className="alert alert-error">{error}</div>}
      {success && <div className="alert alert-success">{success}</div>}

      {/* Submit forms */}
      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '20px', marginBottom: '28px' }}>
        {/* Text report */}
        <div className="card">
          <div className="section-title" style={{ marginBottom: '16px' }}>{editReport ? 'Edit Progress Report' : 'Submit Text Report'}</div>
          <form onSubmit={handleTextSubmit}>
            <div className="form-group">
              <label className="form-label">Project</label>
              <select className="form-control" value={form.projectId} onChange={e => setForm({ ...form, projectId: e.target.value })} disabled={!!editReport}>
                <option value="">— Select Project —</option>
                {projects.map(p => <option key={p.id} value={p.id}>{p.title}</option>)}
              </select>
            </div>
            <div className="form-group">
              <label className="form-label">Progress Report</label>
              <textarea className="form-control" style={{ minHeight: '110px' }} placeholder="Describe your progress this week..." value={form.report} onChange={e => setForm({ ...form, report: e.target.value })} />
            </div>
            <div style={{ display: 'flex', gap: 10 }}>
              <button type="submit" className="btn btn-primary">{editReport ? 'Update Report' : 'Submit Report'}</button>
              {editReport && <button type="button" className="btn btn-secondary" onClick={cancelEdit}>Cancel</button>}
            </div>
          </form>
        </div>

        {/* File upload */}
        <div className="card">
          <div className="section-title" style={{ marginBottom: '16px' }}>Upload File</div>
          <form onSubmit={handleFileUpload}>
            <div className="form-group">
              <label className="form-label">Project</label>
              <select className="form-control" value={form.projectId} onChange={e => setForm({ ...form, projectId: e.target.value })}>
                <option value="">— Select Project —</option>
                {projects.map(p => <option key={p.id} value={p.id}>{p.title}</option>)}
              </select>
            </div>
            <div className="form-group">
              <label className="form-label">File</label>
              <input
                id="file-input"
                type="file"
                className="form-control"
                onChange={e => setFile(e.target.files[0])}
                style={{ paddingTop: '8px' }}
              />
            </div>
            {file && (
              <div style={{ fontSize: '12px', color: 'var(--text-muted)', marginBottom: '12px' }}>
                Selected: <span style={{ color: 'var(--accent-2)' }}>{file.name}</span> ({(file.size / 1024).toFixed(1)} KB)
              </div>
            )}
            <button type="submit" className="btn btn-secondary" disabled={uploading}>
              {uploading ? 'Uploading...' : '↑ Upload File'}
            </button>
          </form>
        </div>
      </div>

      {/* Reports table */}
      {loading ? (
        <div style={{ color: 'var(--text-muted)' }}>Loading...</div>
      ) : reports.length === 0 ? (
        <div className="card" style={{ textAlign: 'center', color: 'var(--text-muted)', padding: '60px' }}>No progress reports yet.</div>
      ) : (
        <div className="table-container">
          <table>
            <thead>
              <tr>
                <th>#</th>
                <th>Project</th>
                <th>Student</th>
                <th>Report</th>
                <th>File</th>
                <th>Date</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {reports.map(r => (
                <tr key={r.id}>
                  <td style={{ color: 'var(--text-muted)' }}>{r.id}</td>
                  <td><strong>{r.projectTitle || '—'}</strong></td>
                  <td>{r.studentName || '—'}</td>
                  <td style={{ maxWidth: '220px', fontSize: '12px', color: 'var(--text-secondary)' }}>
                    <div style={{ overflow: 'hidden', textOverflow: 'ellipsis', whiteSpace: 'nowrap' }}>{r.report || '—'}</div>
                  </td>
                  <td>
                    {r.filePath
                      ? <a href={r.filePath} target="_blank" rel="noreferrer" style={{ color: 'var(--accent-2)', fontSize: '12px' }}>Download</a>
                      : <span style={{ color: 'var(--text-muted)', fontSize: '12px' }}>—</span>}
                  </td>
                  <td style={{ color: 'var(--text-muted)', fontSize: '12px' }}>{new Date(r.dateSubmitted).toLocaleDateString()}</td>
                  <td>
                    <button className="btn btn-secondary btn-sm" onClick={() => openEdit(r)} style={{ marginRight: 6 }}>Edit</button>
                    <button className="btn btn-danger btn-sm" onClick={() => handleDelete(r.id)}>Del</button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}
