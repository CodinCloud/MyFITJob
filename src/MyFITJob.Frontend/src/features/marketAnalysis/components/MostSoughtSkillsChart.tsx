import { Bar, BarChart, CartesianGrid, XAxis } from "recharts";
import { ChartContainer, ChartTooltip, ChartTooltipContent } from "@/components/ui/chart";
import { useMostSoughtSkills } from "../api/marketAnalysisApi";

export function MostSoughtSkillsChart() {
  const { data, isLoading, error } = useMostSoughtSkills();

  if (isLoading) return <div>Chargement...</div>;
  if (error) return <div>Erreur lors du chargement des skills</div>;
  if (!data) return null;

  return (
    <ChartContainer
      config={{
        count: {
          label: "Nombre d'offres",
          color: "var(--chart-2)",
        },
      }}
      className="w-full"
    >
      <BarChart
        data={data}
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
  );
} 