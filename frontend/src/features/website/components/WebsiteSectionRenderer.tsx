import { Card } from "primereact/card";
import type { WebsiteSection } from "../types";

type JsonObject = Record<string, unknown>;

function parseContent(raw: string): JsonObject {
  try {
    return JSON.parse(raw) as JsonObject;
  } catch {
    return {};
  }
}

export function WebsiteSectionRenderer({ section }: { section: WebsiteSection }) {
  const content = parseContent(section.contentJson);

  switch (section.type) {
    case "hero":
      return (
        <Card className="mb-3">
          <h2>{String(content.title ?? "Welcome to our clinic")}</h2>
          <p>{String(content.subtitle ?? "Compassionate care with modern technology.")}</p>
          <strong>{String(content.ctaText ?? "Book Appointment")}</strong>
        </Card>
      );
    case "services":
      return (
        <Card className="mb-3">
          <h3>{String(content.title ?? "Services")}</h3>
          <p>{String(content.description ?? "Describe your services here.")}</p>
        </Card>
      );
    case "doctors":
      return (
        <Card className="mb-3">
          <h3>{String(content.title ?? "Doctors")}</h3>
          <p>{String(content.description ?? "Introduce your doctor team.")}</p>
        </Card>
      );
    case "booking":
      return (
        <Card className="mb-3">
          <h3>{String(content.title ?? "Book Appointment")}</h3>
          <p>{String(content.description ?? "Enable booking in the next phase.")}</p>
        </Card>
      );
    default:
      return (
        <Card className="mb-3">
          <h3>{section.type}</h3>
          <pre>{section.contentJson}</pre>
        </Card>
      );
  }
}
