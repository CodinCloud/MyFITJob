import React from 'react';

type KanbanCard = {
  id: number;
  title: string;
  description: string;
  company: string;
  date: string;
  commentsCount?: number;
  filesCount?: number;
  highlight?: boolean;
};

type KanbanColumnProps = {
  title: string;
  count: number;
  cards: KanbanCard[];
};

export const KanbanColumn: React.FC<KanbanColumnProps> = ({ title, count, cards }) => {
  return (
    <div className="flex flex-col w-80 bg-gray-50 rounded-lg p-4">
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
            className={`bg-white p-4 rounded-lg shadow-sm ${
              card.highlight ? 'border-l-4 border-blue-500' : ''
            }`}
          >
            <h4 className="font-medium text-gray-800">{card.title}</h4>
            <p className="text-sm text-gray-600 mt-1">{card.description}</p>
            <div className="flex justify-between items-center mt-3 text-sm text-gray-500">
              <span>{card.date}</span>
              <div className="flex gap-2">
                {card.commentsCount && (
                  <span className="flex items-center gap-1">
                    💬 {card.commentsCount}
                  </span>
                )}
                {card.filesCount && (
                  <span className="flex items-center gap-1">
                    📎 {card.filesCount}
                  </span>
                )}
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}; 