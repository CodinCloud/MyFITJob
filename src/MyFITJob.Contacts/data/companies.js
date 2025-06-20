const industries = [
  "Tech", "Finance", "Healthcare", "Education", "Retail", 
  "Manufacturing", "Consulting", "Media", "Transportation", "Energy"
];

const companySizes = [
  "1-10", "11-50", "51-200", "201-1000", "1000+"
];

const companies = [
  { id: 1, name: "TechCorp Solutions" },
  { id: 2, name: "InnovateSoft" },
  { id: 3, name: "Digital Dynamics" },
  { id: 4, name: "Future Systems" },
  { id: 5, name: "CloudTech Pro" },
  { id: 6, name: "DataFlow Inc" },
  { id: 7, name: "Smart Solutions" },
  { id: 8, name: "NextGen Tech" },
  { id: 9, name: "CodeCraft Studio" },
  { id: 10, name: "DevHub Labs" }
];

function getRandomCompanyInfo() {
  return {
    industry: industries[Math.floor(Math.random() * industries.length)],
    size: companySizes[Math.floor(Math.random() * companySizes.length)],
    rating: parseFloat((Math.random() * 2 + 3).toFixed(1)) // Rating between 3.0 and 5.0
  };
}

function getCompanyById(id) {
  const company = companies.find(c => c.id === parseInt(id));
  if (!company) {
    return null;
  }
  
  return {
    ...company,
    ...getRandomCompanyInfo()
  };
}

module.exports = {
  getCompanyById,
  getRandomCompanyInfo
}; 