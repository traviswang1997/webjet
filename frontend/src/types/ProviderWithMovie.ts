import type { IMovieSummaryDto } from "./api-client";

export interface ProviderWithMovie extends IMovieSummaryDto {
  providerWithMovieIds: Record<string, string>;
}
