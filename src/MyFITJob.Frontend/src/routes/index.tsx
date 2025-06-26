import { createFileRoute } from '@tanstack/react-router'
import { useState } from 'react'
import { Button } from '@/components/ui/button'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Avatar, AvatarFallback, AvatarImage } from '@/components/ui/avatar'
import { Input } from '@/components/ui/input'
import { Badge } from '@/components/ui/badge'
import { cn } from '@/lib/utils'
import { JobOfferKanban } from '@/features/jobOffers/components/JobOfferKanban'
import { CreateJobOfferModal } from '@/features/jobOffers/components/CreateJobOfferModal'
import { useCreateJobOffer } from '@/features/jobOffers/api/jobOfferQueries'
import { useToast } from '@/components/ui/use-toast'
import { MostSoughtSkillsChart } from '@/features/marketAnalysis'
import type { CreateJobOffer } from '@/features/jobOffers/jobOffersTypes'

import logo from '../assets/mjf_logo2.png'

import { HomeIcon, BriefcaseIcon, UserIcon, SettingsIcon, ChartBarIcon, PlusIcon } from 'lucide-react'

export const Route = createFileRoute('/')({
  component: App,
})

function App() {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const createJobOfferMutation = useCreateJobOffer();
  const { toast } = useToast();

  const handleCreateJobOffer = async (data: CreateJobOffer) => {
    try {
      const result = await createJobOfferMutation.mutateAsync(data);
      if (result.isSuccess) {
        // Succès - afficher un toast et fermer la modal
        toast({
          title: "Succès !",
          description: "L'offre d'emploi a été créée avec succès.",
        });
        setIsModalOpen(false);
      } else {
        // Erreur - afficher le message d'erreur
        toast({
          title: "Erreur",
          description: result.error?.message || "Une erreur est survenue lors de la création de l'offre.",
          variant: "destructive",
        });
        throw result.error;
      }
    } catch (error) {
      console.error('Erreur lors de la création:', error);
      toast({
        title: "Erreur",
        description: "Une erreur inattendue est survenue.",
        variant: "destructive",
      });
      throw error;
    }
  };

  return (
    <div className="flex min-h-screen bg-muted">
      {/* Sidebar */}
      <aside className="w-64 bg-white border-r flex flex-col justify-between">
        <div>
          <div className="px-6 py-4 text-2xl font-bold">
            <img src={logo} alt="logo" width={100} height={100} />
          </div>
          <nav className="px-2 space-y-1">
            <SidebarItem icon={<HomeIcon />} label="Dashboard" active={true} />
            <SidebarItem icon={<BriefcaseIcon />} label="Job Offers" badge="2" />
            <SidebarItem icon={<UserIcon />} label="Contacts" />
            <SidebarItem icon={<ChartBarIcon />} label="Analytics" />
            <SidebarItem icon={<SettingsIcon />} label="Settings" />
          </nav>
        </div>
        <div className="px-6 py-4 flex items-center gap-2 border-t">
          <Avatar className="h-8 w-8">
            <AvatarImage src="/avatars/5.png" />
            <AvatarFallback>IR</AvatarFallback>
          </Avatar>
          <span className="text-sm font-medium">Iona Rollins</span>
        </div>
      </aside>
      {/* Main content */}
      <main className="flex-1 flex flex-col">
        {/* Header */}
        <header className="flex items-center justify-between px-10 py-6 bg-muted border-b">
          <Input placeholder="Search my next offer..." className="w-80" />
          <div className="flex items-center gap-4">
            <Button variant="ghost">Sort by</Button>
            <Button variant="ghost">Filters</Button>
            <Button onClick={() => setIsModalOpen(true)}>
              <PlusIcon className="mr-2 h-4 w-4" />
              Add Job Offer
            </Button>
          </div>
        </header>
        {/* Stats */}
        <section className="px-10 py-6 grid grid-cols-4 gap-6 bg-muted">
          <Card className="col-span-1 flex flex-col items-center justify-center">
            <CardHeader>
              <CardTitle>Top Skills</CardTitle>
            </CardHeader>
            <CardContent className="w-full">
              <MostSoughtSkillsChart />
            </CardContent>
          </Card>
          <Card className="col-span-1">
            <CardHeader>
              <CardTitle>Nouvelles offres</CardTitle>
            </CardHeader>
            <CardContent>
              <div className="text-3xl font-bold">10</div>
            </CardContent>
          </Card>
          <Card className="col-span-1 flex flex-col items-center justify-center">
            <CardContent className="flex flex-col items-center justify-center">
              <div className="text-3xl font-bold">68%</div>
              <div className="text-muted-foreground">Réponses reçues</div>
            </CardContent>
          </Card>
          <Card className="col-span-1 flex flex-col items-center justify-center">
            <CardContent className="flex flex-col items-center justify-center">
              <div className="text-3xl font-bold">53</div>
              <div className="text-muted-foreground">Tâches en cours</div>
            </CardContent>
          </Card>
        </section>
        {/* Kanban */}
        <section className="flex-1 px-10 py-8 bg-white overflow-hidden" style={{ width: 'calc(100vw - 256px)' }}>
          <div className="overflow-x-auto">
            <JobOfferKanban />
          </div>
        </section>
      </main>

      {/* Modal de création d'offre */}
      <CreateJobOfferModal
        open={isModalOpen}
        onOpenChange={setIsModalOpen}
        onSubmit={handleCreateJobOffer}
      />
    </div>
  )
}

function SidebarItem({ icon, label, badge, active }: { icon?: React.ReactNode; label: string; badge?: string; active?: boolean }) {
  return (
    <div className={cn('flex items-center justify-between px-4 py-2 rounded cursor-pointer hover:bg-muted', active && 'bg-muted font-semibold')}> 
      <span className="flex items-center gap-2">{icon && <span>{icon}</span>}{label}</span>
      {badge && <Badge variant="secondary">{badge}</Badge>}
    </div>
  )
}
