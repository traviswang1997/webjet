import { useQuery } from "@tanstack/react-query";
import { MovieDetailsDto } from "../types/api-client";
import { fetchMovieDetails } from "../services/api";

// getting a single movie data
export function useMovie(providerId: string, movieId: string) {
  return useQuery<MovieDetailsDto>({
    queryKey: ["movieDetails", providerId, movieId],
    queryFn: () => fetchMovieDetails(providerId, movieId),
    staleTime: 5 * 60000,
    placeholderData: new MovieDetailsDto(),
    retry: 2,
  });
}
