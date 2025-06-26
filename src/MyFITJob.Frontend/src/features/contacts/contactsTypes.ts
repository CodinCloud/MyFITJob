export type CompanyInfo = {
  industry: string;
  size: string;
  rating: number;
};

export type Company = {
  id: number;
  name: string;
  industry: string;
  size: string;
  rating: number;
  description: string;
};

export type CreateCompany = {
  name: string;
  industry: string;
  size: string;
  rating?: number;
  description?: string;
}; 