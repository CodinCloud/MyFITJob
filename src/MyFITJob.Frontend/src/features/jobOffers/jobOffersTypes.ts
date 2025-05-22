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

export type JobOffer = {
  id: number;
  title: string;
  description: string;
  company: string;
  location: string;
  salary?: string;
  status: JobOfferStatus;
  createdAt: string;
  updatedAt: string;
  lastInteraction?: string;
  commentsCount?: number;
};
