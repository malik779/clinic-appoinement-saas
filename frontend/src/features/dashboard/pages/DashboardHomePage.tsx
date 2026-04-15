import { Card } from "primereact/card";

export function DashboardHomePage() {
  return (
    <div className="grid">
      <div className="col-12 md:col-4">
        <Card title="Tenant Overview">
          <p className="m-0">Monitor clinic activity and tenant health.</p>
        </Card>
      </div>
      <div className="col-12 md:col-4">
        <Card title="Website Status">
          <p className="m-0">Preview and manage landing page sections.</p>
        </Card>
      </div>
      <div className="col-12 md:col-4">
        <Card title="Subscription">
          <p className="m-0">Track plan usage and feature availability.</p>
        </Card>
      </div>
    </div>
  );
}
