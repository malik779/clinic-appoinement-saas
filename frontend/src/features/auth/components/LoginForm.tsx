import { useState } from "react";
import { Button } from "primereact/button";
import { Card } from "primereact/card";
import { Dropdown } from "primereact/dropdown";
import { InputText } from "primereact/inputtext";
import { Password } from "primereact/password";
import { useAuth } from "../../../app/providers/AuthProvider";

const roleOptions = [
  { label: "Tenant Admin", value: "TenantAdmin" },
  { label: "Staff", value: "Staff" },
];

type LoginFormProps = {
  onRegister: (payload: {
    email: string;
    password: string;
    fullName: string;
    role: "TenantAdmin" | "Staff";
  }) => Promise<void>;
};

export function LoginForm({ onRegister }: LoginFormProps) {
  const { login } = useAuth();
  const [tenantId, setTenantId] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [fullName, setFullName] = useState("");
  const [role, setRole] = useState("Staff");
  const [mode, setMode] = useState<"login" | "register">("login");
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  async function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setError(null);
    setLoading(true);

    try {
      if (!tenantId.trim() || !email.trim() || !password.trim()) {
        throw new Error("Tenant, email and password are required.");
      }

      if (mode === "register") {
        await onRegister({
          email,
          password,
          fullName,
          role: role as "TenantAdmin" | "Staff",
        });
      }

      await login({
        tenantId,
        email,
        password,
      });
    } catch (exception) {
      setError(exception instanceof Error ? exception.message : "Request failed.");
    } finally {
      setLoading(false);
    }
  }

  return (
    <Card title="Clinic SaaS Authentication" className="auth-card">
      <form className="p-fluid flex flex-column gap-3" onSubmit={handleSubmit}>
        <div className="field">
          <label htmlFor="tenantId">Tenant Id</label>
          <InputText
            id="tenantId"
            value={tenantId}
            onChange={(event) => setTenantId(event.target.value)}
            placeholder="Tenant GUID from onboarding"
          />
        </div>

        <div className="field">
          <label htmlFor="email">Email</label>
          <InputText
            id="email"
            type="email"
            value={email}
            onChange={(event) => setEmail(event.target.value)}
          />
        </div>

        <div className="field">
          <label htmlFor="password">Password</label>
          <Password
            id="password"
            value={password}
            onChange={(event) => setPassword(event.target.value)}
            feedback={false}
            toggleMask
          />
        </div>

        {mode === "register" ? (
          <>
            <div className="field">
              <label htmlFor="fullName">Full Name</label>
              <InputText
                id="fullName"
                value={fullName}
                onChange={(event) => setFullName(event.target.value)}
              />
            </div>
            <div className="field">
              <label htmlFor="role">Role</label>
              <Dropdown
                id="role"
                value={role}
                options={roleOptions}
                onChange={(event) => setRole(event.value)}
              />
            </div>
          </>
        ) : null}

        {error ? <small className="p-error">{error}</small> : null}

        <div className="flex gap-2">
          <Button type="submit" loading={loading} label={mode === "login" ? "Login" : "Register + Login"} />
          <Button
            type="button"
            text
            severity="secondary"
            label={mode === "login" ? "Need account?" : "Have account?"}
            onClick={() => setMode(mode === "login" ? "register" : "login")}
          />
        </div>
      </form>
    </Card>
  );
}
