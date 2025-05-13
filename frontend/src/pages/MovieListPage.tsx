import { useProviders } from "../hooks/useProviders";
import MovieCard from "../components/MovieCard";
import { Link } from "wouter";
import { useMovieStore } from "../store";
import { useEffect, useMemo, useState } from "react";
import debounce from "lodash.debounce";

const MovieListPage = () => {
  const [searchTerm, setSearchTerm] = useState("");
  const [filterProvider, setFilterProvider] = useState<"all" | string>("all");

  const providerIds = ["cinemaworld", "filmworld"];
  const { isLoading, error, movies } = useProviders(providerIds);
  const updateSelectedMovie = useMovieStore(
    (state) => state.updateSelectedMovie,
  );

  const filtered = useMemo(() => {
    return movies.filter((m) => {
      const matchesSearch = m.title
        .toLowerCase()
        .includes(searchTerm.toLowerCase());
      const matchesProvider =
        filterProvider === "all" || m.providerWithMovieIds[filterProvider];
      return matchesSearch && matchesProvider;
    });
  }, [movies, searchTerm, filterProvider]);

  //debounce
  const handleSelectMovie = useMemo(
    () =>
      debounce((movie) => {
        updateSelectedMovie(movie);
      }, 300),
    [updateSelectedMovie],
  );

  useEffect(() => {
    return () => {
      handleSelectMovie.cancel();
    };
  }, [handleSelectMovie]);

  if (isLoading)
    return (
      <div className="flex items-center justify-center h-64 text-gray-500">
        Loading...
      </div>
    );
  if (error)
    return (
      <div className="flex items-center justify-center h-64 text-red-500">
        Error loading 1 of the cinemas
      </div>
    );

  return (
    <div className="max-w-6xl mx-auto p-6">
      {/* Filters */}
      <div className="flex flex-col sm:flex-row items-center gap-4 mb-6">
        <input
          type="text"
          placeholder="Search movies…"
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          className="flex-1 border border-gray-300 rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
        />

        <select
          value={filterProvider}
          onChange={(e) => setFilterProvider(e.target.value)}
          className="border text-black bg-white border-gray-300 rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
        >
          <option value="all">All Providers</option>
          {providerIds.map((pid) => (
            <option key={pid} value={pid}>
              {pid.charAt(0).toUpperCase() + pid.slice(1)}
            </option>
          ))}
        </select>
      </div>

      {filtered.length === 0 && (
        <div className="text-center text-gray-600 py-10">No movies found.</div>
      )}
      {/* Filter - End */}

      {/* Movie - display */}
      <ul className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
        {filtered.map((m) => {
          const slug = encodeURIComponent(m.title);
          const qs = new URLSearchParams(m.providerWithMovieIds).toString();

          return (
            <li
              className="bg-white rounded-lg overflow-hidden shadow hover:shadow-lg transition-shadow"
              key={m.id}
            >
              <Link
                className="block"
                to={`/movies/${slug}?${qs}`}
                style={{ textDecoration: "none" }}
              >
                <MovieCard
                  provider={m.providerId}
                  title={m.title}
                  poster={m.poster}
                  type={m.type}
                  year={m.year}
                  onClick={() => handleSelectMovie(m)}
                ></MovieCard>
              </Link>
            </li>
          );
        })}
      </ul>
    </div>
  );
};

export default MovieListPage;
