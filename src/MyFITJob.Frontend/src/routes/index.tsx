import { createFileRoute } from '@tanstack/react-router'
import { Button } from '@/components/ui/button'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Avatar, AvatarFallback, AvatarImage } from '@/components/ui/avatar'
import { Input } from '@/components/ui/input'
import { Badge } from '@/components/ui/badge'
import { cn } from '@/lib/utils'
import { JobOfferKanban } from '@/features/jobOffers/components/JobOfferKanban'
import { Bar, BarChart, CartesianGrid, XAxis } from "recharts"
import { ChartContainer, ChartTooltip, ChartTooltipContent, type ChartConfig } from "@/components/ui/chart"

import logo from '../assets/mjf_logo2.png'

import { HomeIcon, BriefcaseIcon, UserIcon, SettingsIcon, ChartBarIcon, PlusIcon } from 'lucide-react'

export const Route = createFileRoute('/')({
  component: App,
})

function App() {
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
            <Button><PlusIcon />  Add Job Offer</Button>
          </div>
        </header>
        {/* Stats */}
        <section className="px-10 py-6 grid grid-cols-4 gap-6 bg-muted">
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
          <Card className="col-span-1 flex flex-col items-center justify-center">
            <CardHeader>
              <CardTitle>Top Skills</CardTitle>
            </CardHeader>
            <CardContent className="w-full">
              <ChartContainer 
                config={{
                  count: {
                    label: "Nombre d'offres",
                    color: "var(--chart-2)",
                  }
                }} 
                className="w-full"
              >
                <BarChart
                  data={[
                    { name: 'React', count: 12, color: "var(--chart-1)" },
                    { name: 'TypeScript', count: 10, color: "var(--chart-2)" },
                    { name: 'Node.js', count: 8, color: "var(--chart-3)" },
                    { name: 'AWS', count: 7, color: "var(--chart-4)" },
                    { name: 'Docker', count: 6, color: "var(--chart-5)" },
                  ]}
                >
                  <XAxis
                        dataKey="name"
                        tickLine={false}
                        tickMargin={10}
                        axisLine={false}
                      />
                  <CartesianGrid vertical={false} />
                  <Bar dataKey="count" fill="var(--chart-1)" radius={[0, 4, 4, 0]} barSize={20} />
                  <ChartTooltip content={<ChartTooltipContent />} />
                </BarChart>
              </ChartContainer>
            </CardContent>
          </Card>
        </section>
        {/* Kanban */}
        <section className="flex-1 px-10 py-8 bg-white overflow-auto">
          <JobOfferKanban />
        </section>
      </main>
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
