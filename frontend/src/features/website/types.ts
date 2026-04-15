export type SectionDto = {
  id: string;
  type: string;
  contentJson: string;
  sortOrder: number;
};

export type PageDto = {
  id: string;
  slug: string;
  title: string;
  sections: SectionDto[];
};

export type WebsiteDto = {
  id: string;
  tenantId: string;
  theme: string;
  pages: PageDto[];
};

export type WebsiteSection = SectionDto;
