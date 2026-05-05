import { LoginForm } from "../components/LoginForm";
import { authService } from "../services";
import type { RegisterUserPayload } from "../types";

const brandHighlights = [
  {
    label: "Unified operations",
    title: "Clinic, subscription, and website tools in one place",
    description:
      "Keep the front desk, admin team, and digital presence aligned from a single calm workspace.",
  },
  {
    label: "Elevated experience",
    title: "A more premium first impression for your team",
    description:
      "Sharper visuals and thoughtful spacing create a login flow that feels modern from the first click.",
  },
];

const trustPillars = ["Tenant-secure access", "Faster staff onboarding", "Polished clinic branding"];

export function LoginPage() {
  async function handleRegister(payload: RegisterUserPayload): Promise<void> {
    await authService.registerUser(payload);
  }

  return (
    <div className="auth-page">
      <div className="auth-shell">
        <section className="auth-hero" aria-label="Clinic SaaS brand overview">
          <div className="auth-brand">
            <div className="auth-brand-mark" aria-hidden="true">
              <span />
              <span />
            </div>
            <div>
              <p className="auth-brand-kicker">Modern clinic workspace</p>
              <h1>Clinic SaaS</h1>
            </div>
          </div>

          <p className="auth-hero-copy">
            Refined operations for clinics that want calmer workflows, clearer collaboration, and a more
            premium digital presence.
          </p>

          <div className="auth-pillars" aria-label="Product strengths">
            {trustPillars.map((pillar) => (
              <span key={pillar}>{pillar}</span>
            ))}
          </div>

          <div className="auth-hero-grid">
            {brandHighlights.map((highlight) => (
              <article key={highlight.label} className="auth-hero-card">
                <span className="auth-hero-label">{highlight.label}</span>
                <h2>{highlight.title}</h2>
                <p>{highlight.description}</p>
              </article>
            ))}
          </div>

          <div className="auth-hero-footer">
            <div>
              <strong>One elegant access point for every clinic workspace</strong>
              <p>Sign in to manage your tenant with a cleaner, more confident start to the day.</p>
            </div>
            <div className="auth-hero-stat">
              <span>1</span>
              <small>workspace for operations, subscriptions, and website control</small>
            </div>
          </div>
        </section>

        <section className="auth-panel">
          <div className="auth-panel-frame">
            <LoginForm onRegister={handleRegister} />
          </div>
        </section>
      </div>
    </div>
  );
}
