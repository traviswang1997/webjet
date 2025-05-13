import { Link } from 'wouter';
import { useQueries } from '@tanstack/react-query';
import type { MovieDetailsDto } from '../types/api-client';
import { fetchMovieDetails } from '../services/api';
import { useMemo } from 'react';

export default function MovieDetailPage() {
  const params = useMemo(
    () => new URLSearchParams(document.location.search),
    []
  );

  const providerIds = ['cinemaworld', 'filmworld'];

  const detailQs = useQueries({
    queries: providerIds.map((pid) => {
      const movieId = params.get(pid);
      return {
        queryKey: ['movieDetail', pid, movieId],
        queryFn: () => fetchMovieDetails(pid, movieId!),
        staleTime: 5 * 60000,
        enabled: Boolean(movieId), // ← only run if we actually have an ID
        retry: false,
      };
    }),
  });

  const isLoading = detailQs.some((q) => q.isLoading);
  const Errors = detailQs.filter((q) => q.isError);
  const hasAnySuccess = detailQs.some((q) => q.isSuccess);

  if (isLoading)
    return (
      <div className='flex items-center justify-center h-64 text-gray-500'>
        Loading movie details...
      </div>
    );
  if (!hasAnySuccess)
    return (
      <div className='flex items-center justify-center h-64 text-red-500'>
        Error loading cinema.
      </div>
    );

  const result = providerIds
    .map((pid, i) => {
      const q = detailQs[i];
      if (q.isSuccess && q.data) {
        return { providerId: pid, detail: q.data };
      }
      return null;
    })
    .filter((x): x is { providerId: string; detail: MovieDetailsDto } => !!x);

  if (!result.length) {
    return <div>Error loading cinema</div>;
  }

  const cheapest = result.reduce((minValue, currentValue) =>
    currentValue.detail.price! < minValue.detail.price!
      ? currentValue
      : minValue
  );

  const others = result.filter((x) => x.providerId !== cheapest.providerId);

  return (
    <div className='max-w-xl mx-auto p-6 bg-white rounded-lg shadow-lg'>
      <div className='text-4xl font-extrabold text-gray-800'>
        {cheapest.detail.title}
      </div>
      <div className='text-gray-600 mt-2'>{cheapest.detail.actors}</div>
      <section className='mb-8 bg-indigo-50 border border-indigo-200 p-5 rounded-lg'>
        <div className='text-2xl font-semibold text-indigo-700 mb-2'>
          Lowest Price
        </div>
        <div className='text-3xl font-bold text-indigo-900'>
          ${cheapest.detail.price?.toFixed(2)}{' '}
          <span className='text-lg font-medium text-gray-600'>
            @ {cheapest.providerId}
          </span>
        </div>
      </section>
      <section className='mb-8'>
        <div className='text-xl font-semibold text-gray-800 mb-3'>
          Also available at
        </div>
        <ul className='space-y-3'>
          {others.map(({ providerId, detail }) => (
            <li
              key={providerId}
              className='flex items-center justify-between bg-gray-100 p-4 rounded-lg'
            >
              <div>
                <span className='text-lg font-medium text-gray-700'>
                  ${detail.price!.toFixed(2)}
                </span>
                {'   '}
                <span className='text-gray-700'>@ {providerId}</span>
              </div>
            </li>
          ))}
        </ul>
      </section>
      {/* Back Link */}
      <Link
        to='/'
        className='underline text-gray-600'
      >
        <svg
          xmlns='http://www.w3.org/2000/svg'
          fill='none'
          viewBox='0 0 24 24'
          strokeWidth={1.5}
          stroke='currentColor'
          className='size-6'
        >
          <path
            strokeLinecap='round'
            strokeLinejoin='round'
            d='M6.75 15.75 3 12m0 0 3.75-3.75M3 12h18'
          />
        </svg>
        Back to movie list
      </Link>
    </div>
  );
}
