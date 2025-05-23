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

export type Skill = {
  id: number;
  name: string;
  description: string;
};

export type CreateSkill = {
  name: string;
  description: string;
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
  status: JobOfferStatus;
  createdAt: string;
  updatedAt: string;
  commentsCount: number;
  skills: Skill[];
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
