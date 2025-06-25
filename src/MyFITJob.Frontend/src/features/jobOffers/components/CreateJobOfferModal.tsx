import React, { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { Loader2 } from 'lucide-react';

import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogFooter,
} from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Textarea } from '@/components/ui/textarea';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import { Badge } from '@/components/ui/badge';
import type { CreateJobOffer } from '../jobOffersTypes';

// Schéma de validation Zod
const createJobOfferSchema = z.object({
  title: z.string().min(1, 'Le titre est requis'),
  company: z.string().min(1, 'L\'entreprise est requise'),
  location: z.string().min(1, 'La localisation est requise'),
  description: z.string().min(1, 'La description est requise'),
  experienceLevel: z.string().min(1, 'Le niveau d\'expérience est requis'),
  contractType: z.string().min(1, 'Le type de contrat est requis'),
  salary: z.string().min(1, 'Le salaire est requis'),
  skills: z.array(z.object({ name: z.string() })).min(1, 'Au moins une compétence est requise'),
});

type CreateJobOfferFormData = z.infer<typeof createJobOfferSchema>;

// Skills pré-définis pour la démo
const PREDEFINED_SKILLS = [
  { name: '.NET' },
  { name: 'React' },
  { name: 'TypeScript' },
  { name: 'Docker' },
];

// Données placeholder
const PLACEHOLDER_DATA: CreateJobOfferFormData = {
  title: 'Développeur Full Stack .NET/React',
  company: 'TechCorp Solutions',
  location: 'Paris, France',
  description: 'Nous recherchons un développeur Full Stack passionné pour rejoindre notre équipe et contribuer au développement d\'applications web modernes.',
  experienceLevel: 'Mid-level',
  contractType: 'CDI',
  salary: '45 000 - 55 000 €',
  skills: [{ name: '.NET' }, { name: 'React' }],
};

interface CreateJobOfferModalProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  onSubmit: (data: CreateJobOffer) => Promise<void>;
}

export function CreateJobOfferModal({ open, onOpenChange, onSubmit }: CreateJobOfferModalProps) {
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [selectedSkills, setSelectedSkills] = useState<{ name: string }[]>(PLACEHOLDER_DATA.skills);

  const {
    register,
    handleSubmit,
    formState: { errors },
    setValue,
    reset,
    watch,
  } = useForm<CreateJobOfferFormData>({
    resolver: zodResolver(createJobOfferSchema),
    defaultValues: PLACEHOLDER_DATA,
  });

  const watchedValues = watch();

  // Pré-remplir les champs avec les données placeholder
  React.useEffect(() => {
    if (open) {
      reset(PLACEHOLDER_DATA);
      setSelectedSkills(PLACEHOLDER_DATA.skills);
    }
  }, [open, reset]);

  const handleSkillToggle = (skillName: string) => {
    setSelectedSkills(prev => {
      const isSelected = prev.some(skill => skill.name === skillName);
      if (isSelected) {
        return prev.filter(skill => skill.name !== skillName);
      } else {
        return [...prev, { name: skillName }];
      }
    });
  };

  const onSubmitForm = async (data: CreateJobOfferFormData) => {
    setIsSubmitting(true);
    try {
      const jobOfferData: CreateJobOffer = {
        ...data,
        skills: selectedSkills.map(skill => ({ name: skill.name, description: '' })),
      };
      await onSubmit(jobOfferData);
      // Ne pas fermer la modal, laisser l'utilisateur voir le loading
    } catch (error) {
      console.error('Erreur lors de la création:', error);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-[600px] max-h-[90vh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle>Créer une nouvelle offre d'emploi</DialogTitle>
        </DialogHeader>

        {isSubmitting ? (
          <div className="flex flex-col items-center justify-center py-12 space-y-4">
            <Loader2 className="h-8 w-8 animate-spin text-primary" />
            <p className="text-sm text-muted-foreground">Création de l'offre en cours...</p>
          </div>
        ) : (
          <form onSubmit={handleSubmit(onSubmitForm)} className="space-y-6">
            {/* Titre */}
            <div className="space-y-2">
              <Label htmlFor="title">Titre du poste *</Label>
              <Input
                id="title"
                placeholder="Ex: Développeur Full Stack .NET/React"
                {...register('title')}
              />
              {errors.title && (
                <p className="text-sm text-red-500">{errors.title.message}</p>
              )}
            </div>

            {/* Entreprise */}
            <div className="space-y-2">
              <Label htmlFor="company">Entreprise *</Label>
              <Input
                id="company"
                placeholder="Ex: TechCorp Solutions"
                {...register('company')}
              />
              {errors.company && (
                <p className="text-sm text-red-500">{errors.company.message}</p>
              )}
            </div>

            {/* Localisation */}
            <div className="space-y-2">
              <Label htmlFor="location">Localisation *</Label>
              <Input
                id="location"
                placeholder="Ex: Paris, France"
                {...register('location')}
              />
              {errors.location && (
                <p className="text-sm text-red-500">{errors.location.message}</p>
              )}
            </div>

            {/* Description */}
            <div className="space-y-2">
              <Label htmlFor="description">Description *</Label>
              <Textarea
                id="description"
                placeholder="Décrivez le poste, les responsabilités, les exigences..."
                rows={4}
                {...register('description')}
              />
              {errors.description && (
                <p className="text-sm text-red-500">{errors.description.message}</p>
              )}
            </div>

            {/* Niveau d'expérience et Type de contrat */}
            <div className="grid grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label htmlFor="experienceLevel">Niveau d'expérience *</Label>
                <Select
                  value={watchedValues.experienceLevel}
                  onValueChange={(value) => setValue('experienceLevel', value)}
                >
                  <SelectTrigger>
                    <SelectValue placeholder="Sélectionner" />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="Junior">Junior</SelectItem>
                    <SelectItem value="Mid-level">Mid-level</SelectItem>
                    <SelectItem value="Senior">Senior</SelectItem>
                    <SelectItem value="Expert">Expert</SelectItem>
                  </SelectContent>
                </Select>
                {errors.experienceLevel && (
                  <p className="text-sm text-red-500">{errors.experienceLevel.message}</p>
                )}
              </div>

              <div className="space-y-2">
                <Label htmlFor="contractType">Type de contrat *</Label>
                <Select
                  value={watchedValues.contractType}
                  onValueChange={(value) => setValue('contractType', value)}
                >
                  <SelectTrigger>
                    <SelectValue placeholder="Sélectionner" />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="CDI">CDI</SelectItem>
                    <SelectItem value="CDD">CDD</SelectItem>
                    <SelectItem value="Freelance">Freelance</SelectItem>
                    <SelectItem value="Stage">Stage</SelectItem>
                    <SelectItem value="Alternance">Alternance</SelectItem>
                  </SelectContent>
                </Select>
                {errors.contractType && (
                  <p className="text-sm text-red-500">{errors.contractType.message}</p>
                )}
              </div>
            </div>

            {/* Salaire */}
            <div className="space-y-2">
              <Label htmlFor="salary">Salaire *</Label>
              <Input
                id="salary"
                placeholder="Ex: 45 000 - 55 000 €"
                {...register('salary')}
              />
              {errors.salary && (
                <p className="text-sm text-red-500">{errors.salary.message}</p>
              )}
            </div>

            {/* Compétences */}
            <div className="space-y-2">
              <Label>Compétences requises *</Label>
              <div className="flex flex-wrap gap-2">
                {PREDEFINED_SKILLS.map((skill) => (
                  <Badge
                    key={skill.name}
                    variant={selectedSkills.some(s => s.name === skill.name) ? "default" : "outline"}
                    className="cursor-pointer hover:bg-primary hover:text-primary-foreground"
                    onClick={() => handleSkillToggle(skill.name)}
                  >
                    {skill.name}
                  </Badge>
                ))}
              </div>
              {errors.skills && (
                <p className="text-sm text-red-500">{errors.skills.message}</p>
              )}
            </div>

            <DialogFooter>
              <Button
                type="button"
                variant="outline"
                onClick={() => onOpenChange(false)}
                disabled={isSubmitting}
              >
                Annuler
              </Button>
              <Button type="submit" disabled={isSubmitting}>
                {isSubmitting ? (
                  <>
                    <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                    Création...
                  </>
                ) : (
                  'Créer l\'offre'
                )}
              </Button>
            </DialogFooter>
          </form>
        )}
      </DialogContent>
    </Dialog>
  );
} 