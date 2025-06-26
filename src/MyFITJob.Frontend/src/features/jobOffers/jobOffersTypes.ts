export enum JobOfferStatus {
  New = 'new',
  Saved = 'saved',
  Applied = 'applied',
  InterviewPlanned = 'interview_planned',
  Interviewed = 'interviewed',
  OfferReceived = 'offer_received',
  Accepted = 'accepted',
  Rejected = 'rejected',
  Archived = 'archived'
}

// Mapping des noms vers les display names pour l'affichage
export const JobOfferStatusDisplayNames: Record<JobOfferStatus, string> = {
  [JobOfferStatus.New]: 'Nouvelle',
  [JobOfferStatus.Saved]: 'Sauvegardée',
  [JobOfferStatus.Applied]: 'Candidature envoyée',
  [JobOfferStatus.InterviewPlanned]: 'Entretien planifié',
  [JobOfferStatus.Interviewed]: 'Entretien réalisé',
  [JobOfferStatus.OfferReceived]: 'Offre reçue',
  [JobOfferStatus.Accepted]: 'Acceptée',
  [JobOfferStatus.Rejected]: 'Refusée',
  [JobOfferStatus.Archived]: 'Archivée'
};

export type Skill = {
  id: number;
  name: string;
  description: string;
};

export type CreateSkill = {
  name: string;
  description: string;
};

// Type pour l'objet JobOfferStatus retourné par l'API
export type JobOfferStatusObject = {
  name: string;
  displayName: string;
};

export type JobOffer = {
  id: number;
  title: string;
  description: string;
  company: string;
  location: string;
  experienceLevel: string;
  contractType: string;
  salary: string;
  status: JobOfferStatusObject; // Maintenant un objet avec name et displayName
  createdAt: string;
  updatedAt: string;
  commentsCount: number;
  skills: Skill[];
  // Informations enrichies de l'entreprise
  companyInfo: CompanyInfos;
};

export type CompanyInfos = {
  id: number;
  name: string;
  description: string;
  industry: string;
  size: string;
  rating: number;
};

export type CreateJobOffer = {
  title: string;
  description: string;
  company: string;
  location: string;
  experienceLevel: string;
  contractType: string;
  salary: string;
  skills: CreateSkill[];
};
