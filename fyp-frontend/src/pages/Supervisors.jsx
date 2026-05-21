import React, { useEffect, useState } from 'react';
import api from '../api';

const MAX_WORKLOAD = 10; // max projects per supervisor for bar scaling

export default function Supervisors() {
  const [supervisors, setSupervisors] = useState([]);
  const [workload, setWorkload] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    Promise.all([
      api.get('/supervisor'),
      api.get('/supervisor/workload'),
    ]).then(([sRes, wRes]) => {
      setSupervisors(sRes.data);
      setWorkload(wRes.data);
    }).catch(() => {}).finally(() => setLoading(false));
  }, []);

  const getWorkload = (id) => workload.find(w => w.id === id) || {};

  if (loading) return <div style={{ color: 'var(--text-muted)', padding: '40px 0' }}>Loading...</div>;

  return (
    <div>
      <h1 className="page-title">Supervisors</h1>

      {supervisors.length === 0 ? (
        <div className="card" style={{ textAlign: 'center', color: 'var(--text-muted)', padding: '60px' }}>
          No supervisors registered yet.
        </div>
      ) : (
        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(340px, 1fr))', gap: '16px' }}>
          {supervisors.map(s => {
            const wl = getWorkload(s.id);
            const total   = wl.totalProjects    || 0;
            const active  = wl.activeProjects   || 0;
            const done    = wl.completedProjects || 0;
            const pct     = Math.min((total / MAX_WORKLOAD) * 100, 100);
            const activePct = total > 0 ? (active / total) * 100 : 0;
            const donePct   = total > 0 ? (done  / total) * 100 : 0;
            const load = pct > 80 ? 'High' : pct > 50 ? 'Medium' : 'Low';
            const loadColor = pct > 80 ? 'var(--accent-3)' : pct > 50 ? '#ffc800' : 'var(--accent-4)';

            return (
              <div className="card" key={s.id}>
                {/* Header */}
                <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', marginBottom: '16px' }}>
                  <div>
                    <div style={{ fontFamily: 'var(--font-display)', fontWeight: 700, fontSize: '15px', color: 'var(--text-primary)', marginBottom: '3px' }}>{s.name}</div>
                    <div style={{ fontSize: '11px', color: 'var(--text-muted)' }}>{s.email}</div>
                  </div>
                  <span style={{ background: 'rgba(124,92,252,0.12)', border: '1px solid rgba(124,92,252,0.2)', borderRadius: '20px', padding: '3px 10px', fontSize: '10px', color: 'var(--accent)', letterSpacing: '0.5px', fontWeight: 700 }}>
                    #{s.id}
                  </span>
                </div>

                {/* Expertise */}
                {s.expertise && (
                  <div style={{ marginBottom: '16px' }}>
                    <div style={{ fontSize: '10px', color: 'var(--text-muted)', letterSpacing: '1px', textTransform: 'uppercase', marginBottom: '6px' }}>Expertise</div>
                    <div style={{ display: 'flex', flexWrap: 'wrap', gap: '6px' }}>
                      {s.expertise.split(',').map((exp, i) => (
                        <span key={i} style={{ background: 'var(--bg-secondary)', border: '1px solid var(--border)', borderRadius: '4px', padding: '2px 8px', fontSize: '11px', color: 'var(--accent-2)' }}>
                          {exp.trim()}
                        </span>
                      ))}
                    </div>
                  </div>
                )}

                {/* Workload bar */}
                <div style={{ marginBottom: '14px' }}>
                  <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '6px' }}>
                    <span style={{ fontSize: '10px', color: 'var(--text-muted)', letterSpacing: '1px', textTransform: 'uppercase' }}>Workload</span>
                    <span style={{ fontSize: '11px', color: loadColor, fontWeight: 700 }}>{load} ({total}/{MAX_WORKLOAD})</span>
                  </div>
                  <div style={{ height: '8px', background: 'var(--bg-secondary)', borderRadius: '4px', overflow: 'hidden' }}>
                    <div style={{ width: `${pct}%`, height: '100%', background: `linear-gradient(90deg, var(--accent), ${loadColor})`, borderRadius: '4px', transition: 'width 0.5s ease' }} />
                  </div>
                </div>

                {/* Active vs Completed bars */}
                <div className="workload-bar-wrap">
                  <div className="workload-bar-label">
                    <span style={{ color: 'var(--accent-4)', fontSize: '11px' }}>Active</span>
                    <span style={{ color: 'var(--text-muted)', fontSize: '11px' }}>{active}</span>
                  </div>
                  <div className="workload-bar-track">
                    <div className="workload-bar-fill" style={{ width: `${activePct}%`, background: 'var(--accent-4)' }} />
                  </div>
                </div>
                <div className="workload-bar-wrap">
                  <div className="workload-bar-label">
                    <span style={{ color: 'var(--accent)', fontSize: '11px' }}>Completed</span>
                    <span style={{ color: 'var(--text-muted)', fontSize: '11px' }}>{done}</span>
                  </div>
                  <div className="workload-bar-track">
                    <div className="workload-bar-fill" style={{ width: `${donePct}%` }} />
                  </div>
                </div>

                {/* Stats row */}
                <div style={{ display: 'flex', gap: '12px', marginTop: '14px', paddingTop: '14px', borderTop: '1px solid var(--border)' }}>
                  {[
                    { label: 'Total', value: total, color: 'var(--text-primary)' },
                    { label: 'Active', value: active, color: 'var(--accent-4)' },
                    { label: 'Done', value: done, color: 'var(--accent)' },
                  ].map(item => (
                    <div key={item.label} style={{ flex: 1, textAlign: 'center', background: 'var(--bg-secondary)', borderRadius: '6px', padding: '8px 4px' }}>
                      <div style={{ fontFamily: 'var(--font-display)', fontWeight: 800, fontSize: '20px', color: item.color }}>{item.value}</div>
                      <div style={{ fontSize: '10px', color: 'var(--text-muted)', letterSpacing: '0.5px' }}>{item.label}</div>
                    </div>
                  ))}
                </div>
              </div>
            );
          })}
        </div>
      )}
    </div>
  );
}
