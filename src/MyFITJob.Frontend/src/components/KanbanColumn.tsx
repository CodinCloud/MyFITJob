import React from 'react';

type KanbanCard = {
  id: number;
  title: string;
  description: string;
  location: string;
  experienceLevel: string;
  contractType: string;
  salary: string;
  date: string;
  commentsCount: number;
  skills: string;
  // Données provenant du microservice de contacts
  companyInfo?: {
    name: string;
    industry: string;
    size: string;
    rating: number;
  };
};

type KanbanColumnProps = {
  title: string;
  count: number;
  cards: KanbanCard[];
};

export const KanbanColumn: React.FC<KanbanColumnProps> = ({ title, count, cards }) => {
  return (
    <div className="flex flex-col w-96 bg-gray-50 rounded-lg p-4">
      <div className="flex justify-between items-center mb-4">
        <h3 className="font-semibold text-gray-700">{title}</h3>
        <span className="bg-gray-200 text-gray-600 px-2 py-1 rounded-full text-sm">
          {count}
        </span>
      </div>
      <div className="flex flex-col gap-3">
        {cards.map((card) => (
          <div
            key={card.id}
            className="bg-white p-4 rounded-lg shadow-sm hover:shadow-md transition-shadow"
          >
            <h4 className="font-medium text-gray-800">{card.title}</h4>
            <p className="text-sm text-gray-600 mt-1 line-clamp-2">{card.description}</p>
            <div className="mt-2">
              <p className="text-sm font-medium text-gray-700">
                {card.companyInfo ? card.companyInfo.name : "Entreprise inconnue"}  
              </p>
              <p className="text-sm text-gray-500">{card.location}</p>
            </div>
            <div className="mt-2 flex flex-wrap gap-2">
              <span className="text-xs bg-blue-100 text-blue-800 px-2 py-1 rounded">
                {card.companyInfo ? `${card.companyInfo.industry} • ${card.companyInfo.size}` : "Indisponible"}
              </span>
              {card.companyInfo && card.companyInfo.rating > 0 && (
                <span className="text-xs bg-yellow-100 text-yellow-800 px-2 py-1 rounded">
                  ⭐ {card.companyInfo.rating.toFixed(1)}
                </span>
              )}
              <span className="text-xs bg-green-100 text-green-800 px-2 py-1 rounded">
                {card.contractType}
              </span>
              <span className="text-xs bg-purple-100 text-purple-800 px-2 py-1 rounded">
                {card.salary}
              </span>
            </div>
            {card.skills && (
              <div className="mt-2">
                <p className="text-xs text-gray-500 line-clamp-1">{card.skills}</p>
              </div>
            )}
            <div className="flex justify-between items-center mt-3 text-sm text-gray-500">
              <span>{card.date}</span>
              {card.commentsCount > 0 && (
                <span className="flex items-center gap-1">
                  💬 {card.commentsCount}
                </span>
              )}
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}; 