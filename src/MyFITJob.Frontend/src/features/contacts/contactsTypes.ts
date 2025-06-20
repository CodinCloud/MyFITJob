export type CompanyInfo = {
  industry: string;
  size: string;
  rating: number;
};

export type Company = {
  id: number;
  name: string;
  info?: CompanyInfo;
};

export type CreateCompany = {
  name: string;
}; 