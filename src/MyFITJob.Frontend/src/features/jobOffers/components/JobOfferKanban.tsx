import React from 'react';
import { KanbanColumn } from '@/components/KanbanColumn';
import { useJobOffers } from '../api/jobOfferQueries';
import { JobOfferStatus, JobOfferStatusDisplayNames } from '../jobOffersTypes';
import type { JobOffer } from '../jobOffersTypes';

export const JobOfferKanban: React.FC = () => {
  const { data: jobOffersResult, isLoading, error } = useJobOffers();

  if (isLoading) {
    return <div>Chargement...</div>;
  }

  if (error) {
    return <div>Erreur lors du chargement des offres d'emploi</div>;
  }

  const jobOffers = (jobOffersResult?.isSuccess) ? jobOffersResult.value : [];

  // Grouper les offres par statut
  const offersByStatus = Object.values(JobOfferStatus).reduce((acc, status) => {
    acc[status] = jobOffers.filter((offer: JobOffer) => offer.status.name === status);
    return acc;
  }, {} as Record<JobOfferStatus, JobOffer[]>);

  // Filtrer les offres archivées
  const filteredOffersByStatus = Object.fromEntries(
    Object.entries(offersByStatus).filter(([status]) => status !== JobOfferStatus.Archived)
  );

  return (
    <div className="flex gap-6 min-w-max">
      {Object.entries(filteredOffersByStatus).map(([status, offers]) => (
        <KanbanColumn
          key={status}
          title={JobOfferStatusDisplayNames[status as JobOfferStatus]}
          count={offers.length}
          cards={offers.map((offer: JobOffer) => ({
            id: offer.id,
            title: offer.title,
            description: offer.description,
            company: offer.company,
            location: offer.location,
            experienceLevel: offer.experienceLevel,
            contractType: offer.contractType,
            salary: offer.salary,
            date: new Date(offer.updatedAt).toLocaleDateString(),
            commentsCount: offer.commentsCount,
            skills: offer.skills.map(skill => skill.name).join(', '),
            companyInfo: offer.companyInfo,
          }))}
        />
      ))}
    </div>
  );
}; 