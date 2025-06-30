const mongoose = require('mongoose');

// Schéma pour les entreprises
const companySchema = new mongoose.Schema({
  name: {
    type: String,
    required: [true, 'Le nom de l\'entreprise est obligatoire'],
    unique: true,
    trim: true
  },
  industry: {
    type: String,
    trim: true
  },
  size: {
    type: String,
    enum: ['1-10', '10-150', '51-200', '201-500', '501-1000', '1000+'],
    default: '1-10'
  },
  rating: {
    type: Number,
    min: 0,
    max: 5,
    default: 0
  },
  description: {
    type: String,
    trim: true,
    default: ''
  }
}, {
  timestamps: true // Ajoute automatiquement createdAt et updatedAt
});

// Index pour optimiser les recherches
companySchema.index({ name: 1 });
companySchema.index({ industry: 1 });

// Méthode d'instance pour formater l'affichage
companySchema.methods.toJSON = function() {
  const company = this.toObject();
  
  // Utiliser _id comme identifiant principal
  company.id = company._id.toString();
  delete company._id;
  delete company.__v;
  
  return company;
};

// Méthode statique pour rechercher par nom (insensible à la casse)
companySchema.statics.findByName = function(name) {
  return this.findOne({ name: new RegExp(name, 'i') });
};

module.exports = mongoose.model('Company', companySchema); 