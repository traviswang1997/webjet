import { useQueries } from "@tanstack/react-query";
import { MovieSummaryDto } from "../types/api-client";
import { fetchMoviesByProvider } from "../services/api";
import { useMemo } from "react";
import type { ProviderWithMovie } from "../types/ProviderWithMovie";


//loading cinemas data for all movies
export function useProviders(providerIds: string[]): {
  isLoading: boolean;
  error: unknown;
  movies: ProviderWithMovie[];
} {
  const results = useQueries({
    queries: providerIds.map((providerId) => ({
      queryKey: ["provider", providerId],
      queryFn: () =>
        fetchMoviesByProvider(providerId).then(
          (wrapper) => wrapper.providerDto,
        ),
      staleTime: 5 * 60000,
      cacheTime: 5 * 60000,
      retry: 2,
    })),
  });

  const isLoading = results.some((r) => r.isLoading);
  const error = results.every((r) => r.isError);
  const summariesDeps = results.map((r) => r.data?.movieSummaries);

  const movies = useMemo(() => {
    const map: Record<string, ProviderWithMovie> = {};

    results.forEach((res, i) => {
      const pid = providerIds[i];
      const data = res.data?.movieSummaries;
      if (!data) return;

      data.forEach((m: MovieSummaryDto) => {
        if (!map[m.title]) {
          map[m.title] = { ...m, providerWithMovieIds: { [pid]: m.id } };
        } else {
          map[m.title].providerWithMovieIds[pid] = m.id;
        }
      });
    });

    return Object.values(map);
  }, [providerIds.length, ...summariesDeps]);
  //   const map: Record<string, MovieSummaryDto> = {};

  //   for (let id = 0; id < providerIds.length; id++) {
  //     const res = results[id];
  //     if (res.data) {
  //       res.data.movieSummaries?.forEach((movie: MovieSummaryDto) => {
  //         if (!map[movie.title]) {
  //           map[movie.title] = movie;
  //         }
  //         if (!map[movie.title].availableIn.includes(movie.providerId)) {
  //           map[movie.title].availableIn.push(movie.providerId);
  //         }
  //       });
  //     }
  //   }
  //   return Object.values(map);
  // }, [providerIds.length, results]);

  return { isLoading, error, movies };
}
