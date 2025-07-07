import { http, HttpResponse } from 'msw'

export const handlers = [
  // Mock pour les job offers
  http.get('/api/jobOffers', () => {
    return HttpResponse.json([
      {
        id: 1,
        title: 'Développeur Full Stack React/Node.js',
        company: 'TechCorp',
        location: 'Paris, France',
        salary: '45k-65k €',
        type: 'CDI',
        description: 'Nous recherchons un développeur full stack pour rejoindre notre équipe...',
        skills: ['React', 'Node.js', 'TypeScript', 'MongoDB'],
        status: {
          'name': 'new',
        },
        createdAt: '2024-01-15T10:00:00Z'
      },
      {
        id: 2,
        title: 'DevOps Engineer',
        company: 'CloudTech',
        location: 'Lyon, France',
        salary: '50k-70k €',
        type: 'CDI',
        description: 'Expertise en Docker, Kubernetes et CI/CD requise...',
        skills: ['Docker', 'Kubernetes', 'AWS', 'Jenkins'],
        status: {
          'name': 'new',
        },
        createdAt: '2024-01-14T14:30:00Z'
      },
      {
        id: 3,
        title: 'Data Scientist Python',
        company: 'DataLab',
        location: 'Toulouse, France',
        salary: '55k-75k €',
        type: 'CDI',
        description: 'Analyse de données et machine learning...',
        skills: ['Python', 'Pandas', 'Scikit-learn', 'SQL'],
        status: {
          'name': 'new',
        },
        createdAt: '2024-01-13T09:15:00Z'
      },
      {
        id: 4,
        title: 'Frontend Developer Vue.js',
        company: 'WebAgency',
        location: 'Bordeaux, France',
        salary: '40k-60k €',
        type: 'CDI',
        description: 'Développement d\'interfaces utilisateur modernes...',
        skills: ['Vue.js', 'JavaScript', 'CSS3', 'Git'],
        status: {
          'name': 'new',
        },
        createdAt: '2024-01-12T16:45:00Z'
      }
    ])
  }),

  // Mock pour l'analyse de marché
  http.get('/api/market/skills?top=5', () => {
    return HttpResponse.json({
      mostSoughtSkills: [
        { skill: 'React', demand: 85 },
        { skill: 'Node.js', demand: 78 },
        { skill: 'Python', demand: 72 },
        { skill: 'Docker', demand: 68 },
        { skill: 'TypeScript', demand: 65 }
      ],
      averageSalary: 55000,
      totalOffers: 1247
    })
  })
] 