import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import { AuthProvider, useAuth } from './context/AuthContext'
import Login       from './pages/Login'
import Register    from './pages/Register'
import Dashboard   from './pages/Dashboard'
import Projects    from './pages/Projects'
import Groups      from './pages/Groups'
import Proposals   from './pages/Proposals'
import Progress    from './pages/Progress'
import Deliverables from './pages/Deliverables'
import Evaluations from './pages/Evaluations'
import Supervisors from './pages/Supervisors'
import Layout      from './components/Layout'

function ProtectedRoute({ children }) {
  const { user } = useAuth()
  return user ? children : <Navigate to="/" replace />
}

function AppRoutes() {
  const { user } = useAuth()
  return (
    <Routes>
      <Route path="/"           element={user ? <Navigate to="/dashboard" replace /> : <Login />} />
      <Route path="/login"      element={user ? <Navigate to="/dashboard" replace /> : <Login />} />
      <Route path="/register"   element={<Register />} />
      <Route path="/dashboard"  element={<ProtectedRoute><Layout><Dashboard   /></Layout></ProtectedRoute>} />
      <Route path="/groups"     element={<ProtectedRoute><Layout><Groups      /></Layout></ProtectedRoute>} />
      <Route path="/projects"   element={<ProtectedRoute><Layout><Projects    /></Layout></ProtectedRoute>} />
      <Route path="/proposals"  element={<ProtectedRoute><Layout><Proposals   /></Layout></ProtectedRoute>} />
      <Route path="/progress"   element={<ProtectedRoute><Layout><Progress    /></Layout></ProtectedRoute>} />
      <Route path="/deliverables" element={<ProtectedRoute><Layout><Deliverables /></Layout></ProtectedRoute>} />
      <Route path="/evaluations" element={<ProtectedRoute><Layout><Evaluations /></Layout></ProtectedRoute>} />
      <Route path="/supervisors" element={<ProtectedRoute><Layout><Supervisors /></Layout></ProtectedRoute>} />
    </Routes>
  )
}

export default function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <AppRoutes />
      </BrowserRouter>
    </AuthProvider>
  )
}
