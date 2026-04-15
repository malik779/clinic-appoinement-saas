import { Navigate, Outlet, Route, Routes } from "react-router-dom";
import { DashboardLayout } from "../layout/DashboardLayout";
import { useAuth } from "../providers/AuthProvider";
import { LoginPage } from "../../features/auth/pages/LoginPage";
import { DashboardHomePage } from "../../features/dashboard/pages/DashboardHomePage";
import { ClinicPage } from "../../features/clinic/pages/ClinicPage";
import { SubscriptionPage } from "../../features/subscription/pages/SubscriptionPage";
import { WebsitePreviewPage } from "../../features/website/pages/WebsitePreviewPage";

function ProtectedRoute() {
  const { isAuthenticated } = useAuth();

  if (!isAuthenticated) {
    return <Navigate to="/auth/login" replace />;
  }

  return <Outlet />;
}

export function AppRouter() {
  return (
    <Routes>
      <Route path="/auth" element={<Navigate to="/auth/login" replace />} />
      <Route path="/auth/login" element={<LoginPage />} />

      <Route element={<ProtectedRoute />}>
        <Route element={<DashboardLayout />}>
          <Route path="/" element={<DashboardHomePage />} />
          <Route path="/clinic" element={<ClinicPage />} />
          <Route path="/subscription" element={<SubscriptionPage />} />
          <Route path="/website" element={<WebsitePreviewPage />} />
        </Route>
      </Route>

      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
}
