import { create } from "zustand";
import type { ProviderWithMovie } from "./types/ProviderWithMovie";

type MovieStore = {
  selectedMovie: ProviderWithMovie | undefined;
  updateSelectedMovie: (selectedMovie: ProviderWithMovie) => void;
  clear: () => void;
};

export const useMovieStore = create<MovieStore>((set) => ({
  selectedMovie: undefined,
  updateSelectedMovie: (selectedMovie: ProviderWithMovie) =>
    set(() => ({ selectedMovie: selectedMovie })),
  clear: () => set({ selectedMovie: undefined }),
}));
