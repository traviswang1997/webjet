import React, { type PropsWithChildren } from "react";
import { MovieSummaryDto } from "../types/api-client";

interface Props extends Pick<MovieSummaryDto, "title" | "poster" | "year"> {
  type?: string | undefined;
  provider: string;
  onClick: () => void;
}

const MovieCard: React.FC<PropsWithChildren<Props>> = ({
  title,
  poster,
  year,
  type,
  provider,
  onClick,
  children,
}) => (
  <div
    className="cursor-pointer bg-white rounded-lg overflow-hidden shadow hover:shadow-lg transition-shadow"
    onClick={onClick}
  >
    <div className="relative w-full pb-[150%] bg-gray-100">
      <img
        src={poster}
        alt={`${title} poster`}
        className="absolute top-0 left-0 w-full h-full object-cover"
      />
    </div>
    <div className="text-2xl">
      {title} ({year})
    </div>
    <p className="text-sm text-gray-700 mb-1">Type: {type}</p>
    <p>Cinema: {provider}</p>
    {children}
  </div>
);

export default MovieCard;
