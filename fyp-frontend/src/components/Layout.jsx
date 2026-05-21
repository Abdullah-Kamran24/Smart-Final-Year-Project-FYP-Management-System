import { Link, useLocation } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'
import { useState } from 'react'

const navItems = [
  { path: '/dashboard',    label: 'Dashboard',    icon: '📊', roles: ['Admin','Student','Supervisor'] },
  { path: '/groups',       label: 'Groups',       icon: '👥', roles: ['Admin','Student'] },
  { path: '/projects',     label: 'Projects',     icon: '📁', roles: ['Admin','Student','Supervisor'] },
  { path: '/proposals',    label: 'Proposals',    icon: '📋', roles: ['Admin','Student','Supervisor'] },
  { path: '/progress',     label: 'Progress',     icon: '📈', roles: ['Admin','Student','Supervisor'] },
  { path: '/deliverables', label: 'Deliverables', icon: '📦', roles: ['Admin','Student'] },
  { path: '/evaluations',  label: 'Evaluations',  icon: '🎯', roles: ['Admin','Supervisor'] },
  { path: '/supervisors',  label: 'Supervisors',  icon: '👨‍🏫', roles: ['Admin'] }
]

export default function Layout({ children }) {
  const { user, logout } = useAuth()
  const location = useLocation()
  const [collapsed, setCollapsed] = useState(false)

  const allowed = navItems.filter(n => n.roles.includes(user?.role || ''))

  return (
    <div style={{ display: 'flex', minHeight: '100vh' }}>
      {/* Sidebar */}
      <aside style={{
        width: collapsed ? '68px' : '220px',
        background: '#111113',
        borderRight: '1px solid rgba(255,255,255,0.06)',
        display: 'flex',
        flexDirection: 'column',
        transition: 'width 0.25s ease',
        flexShrink: 0,
        position: 'sticky',
        top: 0,
        height: '100vh',
        overflowY: 'auto'
      }}>

        {/* Logo */}
        <div style={{
          padding: collapsed ? '20px 12px' : '20px 16px',
          borderBottom: '1px solid rgba(255,255,255,0.06)',
          display: 'flex',
          alignItems: 'center',
          gap: '10px'
        }}>
          <div style={{
            width: 32, height: 32,
            background: 'linear-gradient(135deg, #3b82f6, #8b5cf6)',
            borderRadius: 8,
            display: 'flex', alignItems: 'center', justifyContent: 'center',
            fontSize: '14px', flexShrink: 0
          }}>🎓</div>
          {!collapsed && (
            <div>
              <div style={{ fontWeight: 700, fontSize: '14px', letterSpacing: '-0.3px', color: '#fafafa' }}>FYP System</div>
              <div style={{ fontSize: '11px', color: '#71717a' }}>Management Portal</div>
            </div>
          )}
        </div>

        {/* User pill */}
        {!collapsed && (
          <div style={{ padding: '14px 16px', borderBottom: '1px solid rgba(255,255,255,0.06)' }}>
            <div style={{ display: 'flex', alignItems: 'center', gap: '10px' }}>
              <div style={{
                width: 30, height: 30,
                background: 'linear-gradient(135deg, #3b82f6, #8b5cf6)',
                borderRadius: '50%',
                display: 'flex', alignItems: 'center', justifyContent: 'center',
                fontSize: '12px', fontWeight: 700, color: '#fff', flexShrink: 0
              }}>
                {user?.name?.[0]?.toUpperCase()}
              </div>
              <div style={{ overflow: 'hidden' }}>
                <div style={{ fontSize: '13px', fontWeight: 600, color: '#fafafa', whiteSpace: 'nowrap', overflow: 'hidden', textOverflow: 'ellipsis' }}>
                  {user?.name}
                </div>
                <div style={{
                  fontSize: '11px',
                  color: '#3b82f6',
                  fontWeight: 500,
                }}>{user?.role}</div>
              </div>
            </div>
          </div>
        )}

        {/* Nav links */}
        <nav style={{ flex: 1, padding: '12px 8px', display: 'flex', flexDirection: 'column', gap: '2px' }}>
          {allowed.map(item => {
            const active = location.pathname === item.path
            return (
              <Link
                key={item.path}
                to={item.path}
                style={{
                  display: 'flex',
                  alignItems: 'center',
                  gap: '10px',
                  padding: collapsed ? '10px' : '9px 12px',
                  borderRadius: 8,
                  textDecoration: 'none',
                  color: active ? '#fafafa' : '#a1a1aa',
                  background: active ? 'rgba(59,130,246,0.1)' : 'transparent',
                  fontWeight: active ? 600 : 400,
                  fontSize: '13px',
                  transition: 'all 0.15s ease',
                  justifyContent: collapsed ? 'center' : 'flex-start'
                }}
              >
                <span style={{ fontSize: '15px', flexShrink: 0 }}>{item.icon}</span>
                {!collapsed && item.label}
              </Link>
            )
          })}
        </nav>

        {/* Bottom */}
        <div style={{ padding: '12px 8px', borderTop: '1px solid rgba(255,255,255,0.06)', display: 'flex', flexDirection: 'column', gap: '6px' }}>
          <button
            onClick={() => setCollapsed(c => !c)}
            style={{
              background: 'transparent',
              border: '1px solid rgba(255,255,255,0.06)',
              borderRadius: 8, padding: '8px',
              cursor: 'pointer', color: '#71717a', fontSize: '12px',
              fontFamily: 'var(--font)', transition: 'all 0.15s'
            }}
          >
            {collapsed ? '→' : '← Collapse'}
          </button>
          <button
            onClick={logout}
            style={{
              background: 'rgba(239,68,68,0.06)',
              border: '1px solid rgba(239,68,68,0.12)',
              borderRadius: 8, padding: '8px',
              cursor: 'pointer', color: '#ef4444',
              fontSize: '12px', fontFamily: 'var(--font)',
              display: 'flex', alignItems: 'center', justifyContent: 'center', gap: '6px',
              transition: 'all 0.15s'
            }}
          >
            {collapsed ? '→' : 'Sign out'}
          </button>
        </div>
      </aside>

      {/* Main content */}
      <main style={{ flex: 1, padding: '28px 32px', overflowY: 'auto', minWidth: 0 }}>
        {children}
      </main>
    </div>
  )
}
