import type { MovieDetailsDto } from "../types/api-client";
import type { PagedProviderResponse } from "../types/PagedProviderResponse";
import { API_BASE_URL } from "../utils/config";

//Fetch movie by provider id
export const fetchMoviesByProvider = async (
  providerId: string,
  forceRefresh: boolean = false,
  page: number = 1,
  pageSize: number = 10,
): Promise<PagedProviderResponse> => {
  const url = new URL(`${API_BASE_URL}/api/providers/${providerId}/movies`);
  url.searchParams.set("forceRefresh", String(forceRefresh));
  url.searchParams.set("page", String(page));
  url.searchParams.set("pageSize", String(pageSize));

  const res = await fetch(url.toString());
  if (!res.ok) throw new Error(`Error ${res.status}`);
  return res.json();
};

//Fetch movie details by movie id
export const fetchMovieDetails = async (
  providerId: string,
  movieId: string,
): Promise<MovieDetailsDto> => {
  const url = new URL(`${API_BASE_URL}/api/movies/${movieId}`);
  url.searchParams.set("providerId", providerId);
  const res = await fetch(url.toString());

  if (!res.ok) throw new Error(`Error ${res.status}`);
  return res.json();
};
