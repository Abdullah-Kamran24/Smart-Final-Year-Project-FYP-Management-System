import { useEffect, useState } from 'react'
import { Bar, Doughnut } from 'react-chartjs-2'
import {
  Chart as ChartJS,
  CategoryScale, LinearScale, BarElement,
  Title, Tooltip, Legend, ArcElement
} from 'chart.js'
import api from '../api'
import { useAuth } from '../context/AuthContext'

ChartJS.register(CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend, ArcElement)

export default function Dashboard() {
  const { user }              = useAuth()
  const [stats, setStats]     = useState(null)
  const [myGroup, setMyGroup] = useState(null)
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    const fetchAll = async () => {
      try {
        const r = await api.get('/project/stats')
        setStats(r.data)

        // If student, also fetch their group info
        if (user?.role === 'Student') {
          try {
            const g = await api.get(`/group/student/${user.userId}`)
            setMyGroup(g.data)
          } catch {}
        }
      } catch {}
      setLoading(false)
    }
    fetchAll()
  }, [])

  if (loading) return (
    <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', height: '60vh' }}>
      <div style={{ textAlign: 'center', color: 'var(--text-muted)' }}>
        <div style={{ fontSize: '2rem', marginBottom: '0.5rem' }}>⏳</div>
        <div>Loading dashboard…</div>
      </div>
    </div>
  )

  const barData = {
    labels: ['Projects', 'Students', 'Supervisors', 'Groups', 'Completed'],
    datasets: [{
      label: 'Count',
      data: [
        stats?.totalProjects,
        stats?.totalStudents,
        stats?.totalSupervisors,
        stats?.totalGroups,
        stats?.completedProjects
      ],
      backgroundColor: [
        'rgba(59,130,246,0.7)',
        'rgba(99,102,241,0.7)',
        'rgba(16,185,129,0.7)',
        'rgba(245,158,11,0.7)',
        'rgba(239,68,68,0.7)'
      ],
      borderColor: ['#3b82f6','#6366f1','#10b981','#f59e0b','#ef4444'],
      borderWidth: 2,
      borderRadius: 8,
    }]
  }

  const doughnutData = {
    labels: ['Approved', 'Pending', 'Rejected'],
    datasets: [{
      data: [stats?.approvedProposals, stats?.pendingProposals, stats?.rejectedProposals],
      backgroundColor: ['rgba(16,185,129,0.8)','rgba(245,158,11,0.8)','rgba(239,68,68,0.8)'],
      borderColor: ['#10b981','#f59e0b','#ef4444'],
      borderWidth: 2,
    }]
  }

  const chartOptions = {
    responsive: true,
    plugins: {
      legend: { labels: { color: '#94a3b8', font: { family: 'Sora' } } },
    },
    scales: {
      y: { grid: { color: 'rgba(255,255,255,0.05)' }, ticks: { color: '#94a3b8' } },
      x: { grid: { display: false }, ticks: { color: '#94a3b8' } }
    }
  }

  const doughnutOptions = {
    responsive: true,
    plugins: {
      legend: { position: 'bottom', labels: { color: '#94a3b8', font: { family: 'Sora' }, padding: 16 } }
    }
  }

  return (
    <div className="fade-in">
      <div className="page-header">
        <h1 className="page-title">👋 Welcome back, {user?.name?.split(' ')[0]}</h1>
        <p className="page-sub">Here's what's happening with your FYP system today.</p>
      </div>

      {/* ── Student: My Group Banner ── */}
      {user?.role === 'Student' && myGroup && (
        <div className="card" style={{ marginBottom: '1.5rem', background: 'rgba(59,130,246,0.06)', border: '1px solid rgba(59,130,246,0.2)' }}>
          <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', flexWrap: 'wrap', gap: '1rem' }}>
            <div>
              <div style={{ fontSize: '0.75rem', color: 'var(--text-muted)', textTransform: 'uppercase', letterSpacing: '0.05em', marginBottom: '0.25rem' }}>My Group</div>
              <div style={{ fontWeight: 800, fontSize: '1.1rem' }}>👥 {myGroup.groupName}</div>
              <div style={{ fontSize: '0.8rem', color: 'var(--text-muted)', marginTop: '0.25rem' }}>
                Members: {myGroup.members?.map(m => m.studentName).join(' · ')}
              </div>
            </div>
            <div style={{ textAlign: 'right' }}>
              {myGroup.projectTitle && (
                <div>
                  <div style={{ fontSize: '0.75rem', color: 'var(--text-muted)', textTransform: 'uppercase', letterSpacing: '0.05em' }}>Project</div>
                  <div style={{ fontWeight: 600, fontSize: '0.9rem' }}>📁 {myGroup.projectTitle}</div>
                </div>
              )}
              {myGroup.supervisorName && (
                <div style={{ fontSize: '0.8rem', color: 'var(--green)', marginTop: '0.25rem' }}>
                  👨‍🏫 {myGroup.supervisorName}
                </div>
              )}
            </div>
          </div>
        </div>
      )}

      {/* ── Main stat cards ── */}
      <div className="grid-4" style={{ marginBottom: '1.5rem' }}>
        <div className="stat-card">
          <div className="stat-number">{stats?.totalProjects ?? 0}</div>
          <div className="stat-label">Total Projects</div>
        </div>
        <div className="stat-card">
          <div className="stat-number">{stats?.totalGroups ?? 0}</div>
          <div className="stat-label">Total Groups</div>
        </div>
        <div className="stat-card">
          <div className="stat-number">{stats?.totalStudents ?? 0}</div>
          <div className="stat-label">Students</div>
        </div>
        <div className="stat-card">
          <div className="stat-number">{stats?.totalSupervisors ?? 0}</div>
          <div className="stat-label">Supervisors</div>
        </div>
      </div>

      {/* ── Secondary stats ── */}
      <div className="grid-3" style={{ marginBottom: '1.5rem' }}>
        <div className="card" style={{ display: 'flex', alignItems: 'center', gap: '1rem' }}>
          <div style={{ fontSize: '2rem' }}>✅</div>
          <div>
            <div style={{ fontSize: '1.5rem', fontWeight: 800, color: 'var(--green)' }}>{stats?.approvedProposals ?? 0}</div>
            <div style={{ fontSize: '0.75rem', color: 'var(--text-muted)', textTransform: 'uppercase', letterSpacing: '0.05em' }}>Approved Proposals</div>
          </div>
        </div>
        <div className="card" style={{ display: 'flex', alignItems: 'center', gap: '1rem' }}>
          <div style={{ fontSize: '2rem' }}>⚡</div>
          <div>
            <div style={{ fontSize: '1.5rem', fontWeight: 800, color: 'var(--accent)' }}>{stats?.activeProjects ?? 0}</div>
            <div style={{ fontSize: '0.75rem', color: 'var(--text-muted)', textTransform: 'uppercase', letterSpacing: '0.05em' }}>Active Projects</div>
          </div>
        </div>
        <div className="card" style={{ display: 'flex', alignItems: 'center', gap: '1rem' }}>
          <div style={{ fontSize: '2rem' }}>🏁</div>
          <div>
            <div style={{ fontSize: '1.5rem', fontWeight: 800, color: 'var(--yellow)' }}>{stats?.completedProjects ?? 0}</div>
            <div style={{ fontSize: '0.75rem', color: 'var(--text-muted)', textTransform: 'uppercase', letterSpacing: '0.05em' }}>Completed Projects</div>
          </div>
        </div>
      </div>

      {/* ── Charts ── */}
      <div className="grid-2">
        <div className="card">
          <h3 style={{ fontSize: '0.9rem', fontWeight: 700, marginBottom: '1.25rem', color: 'var(--text-muted)', textTransform: 'uppercase', letterSpacing: '0.05em' }}>
            📊 System Overview
          </h3>
          <Bar data={barData} options={chartOptions} />
        </div>
        <div className="card">
          <h3 style={{ fontSize: '0.9rem', fontWeight: 700, marginBottom: '1.25rem', color: 'var(--text-muted)', textTransform: 'uppercase', letterSpacing: '0.05em' }}>
            📋 Proposal Status
          </h3>
          <div style={{ maxWidth: 280, margin: '0 auto' }}>
            <Doughnut data={doughnutData} options={doughnutOptions} />
          </div>
        </div>
      </div>

      {/* Info footer */}
      <div className="card" style={{ marginTop: '1.5rem', background: 'rgba(59,130,246,0.05)', border: '1px solid rgba(59,130,246,0.15)' }}>
        <div style={{ display: 'flex', alignItems: 'center', gap: '0.75rem', flexWrap: 'wrap' }}>
          <span style={{ fontSize: '1.25rem' }}>🎓</span>
          <div>
            <div style={{ fontWeight: 700, fontSize: '0.9rem' }}>Smart FYP Management System — Group Architecture</div>
            <div style={{ fontSize: '0.75rem', color: 'var(--text-muted)', fontFamily: 'var(--mono)' }}>
              Mehaal Khan (23P-0544) · Abdullah Kamran (23P-0612) · Mustafa Naeem (23P-0501) · Database Course
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}