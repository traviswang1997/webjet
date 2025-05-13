import { useQuery } from "@tanstack/react-query";

export type ProviderHealth = { provider: string; isHealthy: boolean };

//server status
export const useProvidersHealth = () => {
  return useQuery<ProviderHealth[]>({
    queryKey: ["providers-health"],
    queryFn: async () => {
      const res = await fetch("http://localhost:5260/api/health");
      if (!res.ok) throw new Error("Failed to fetch provider health");
      return res.json();
    },
    staleTime: 5 * 60 * 1000, // 5 minutes
    retry: 1,
  });
};
