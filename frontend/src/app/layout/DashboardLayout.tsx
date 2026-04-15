import { Link, Navigate, Outlet, useLocation } from "react-router-dom";
import { useAuth } from "../providers/AuthProvider";

const menuItems = [
  { path: "/", label: "Dashboard" },
  { path: "/clinic", label: "Clinic" },
  { path: "/subscription", label: "Subscription" },
  { path: "/website", label: "Website Builder" },
];

export function DashboardLayout() {
  const { isAuthenticated, email, tenantId, logout } = useAuth();
  const location = useLocation();

  if (!isAuthenticated) {
    return <Navigate to="/auth/login" replace />;
  }

  return (
    <div className="dashboard-layout">
      <aside className="sidebar">
        <h2>Clinic SaaS</h2>
        <p className="tenant-info">Tenant: {tenantId}</p>
        <nav>
          {menuItems.map((item) => (
            <Link
              key={item.path}
              to={item.path}
              className={location.pathname.startsWith(item.path) ? "active" : ""}
            >
              {item.label}
            </Link>
          ))}
        </nav>
        <button type="button" className="logout-button" onClick={logout}>
          Sign Out
        </button>
      </aside>
      <main>
        <header className="topbar">Signed in as {email}</header>
        <div className="page-container">
          <Outlet />
        </div>
      </main>
    </div>
  );
}
