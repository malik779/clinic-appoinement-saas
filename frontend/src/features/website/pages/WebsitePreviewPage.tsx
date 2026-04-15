import { useQuery } from "@tanstack/react-query";
import { Message } from "primereact/message";
import { ProgressSpinner } from "primereact/progressspinner";
import { websiteService } from "../services";
import { WebsiteSectionRenderer } from "../components/WebsiteSectionRenderer";

export function WebsitePreviewPage() {
  const { data, isLoading, isError } = useQuery({
    queryKey: ["website-preview"],
    queryFn: websiteService.getCurrentTenantWebsite,
  });

  if (isLoading) {
    return (
      <div className="page-center">
        <ProgressSpinner />
      </div>
    );
  }

  if (isError || !data) {
    return (
      <Message
        severity="error"
        text="Failed to load website preview. Ensure tenant header and authentication are valid."
      />
    );
  }

  const homePage = data.pages.find((page) => page.slug === "home") ?? data.pages[0];

  return (
    <div>
      <h2>Website Preview</h2>
      <p className="section-subtitle">Theme: {data.theme}</p>
      {homePage ? (
        <div className="flex flex-column gap-3">
          {homePage.sections.map((section) => (
            <WebsiteSectionRenderer key={section.id} section={section} />
          ))}
        </div>
      ) : (
        <Message severity="warn" text="No pages available for this tenant website." />
      )}
    </div>
  );
}
