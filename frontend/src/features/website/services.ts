import { apiGet } from "../../shared/api/api";
import type { WebsiteDto } from "./types";

export const websiteService = {
  getCurrentTenantWebsite(): Promise<WebsiteDto> {
    return apiGet<WebsiteDto>("/website/current");
  },
};
