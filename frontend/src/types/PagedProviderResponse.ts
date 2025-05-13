import type { ProviderDto } from "./api-client";

export interface PagedProviderResponse {
  page: number;
  pageSize: number;
  total: number;
  providerDto: ProviderDto;
}
