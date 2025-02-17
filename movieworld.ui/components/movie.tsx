import Image from "next/image";

type Movie = {
    id: string;
    title: string;
    rating: string;
    price: number;
    poster: string;  
  };
  
  type MovieCardProps = {
    movie: Movie;
  };
  
  const MovieCard: React.FC<MovieCardProps> = ({ movie }) => {
    return (
      <div className="bg-white p-6 rounded-lg shadow-md hover:shadow-xl transition-shadow bg-gradient-to-r from-red-50 via-red-100 to-red-200">
        <h2 className="text-2xl font-semibold text-primary">{movie.title}</h2>
        <p className="text-lg text-gray-700">Rating: {movie.rating}</p>
        <p className="text-lg text-gray-700">Price: ${movie.price.toFixed(2)}</p>
        

        <Image
          src={movie.poster} 
          alt={movie.title}
          width={300} 
          height={450}
          className="w-full h-auto rounded-lg"
        />
      </div>
    );
  };
  
  export default MovieCard;
  