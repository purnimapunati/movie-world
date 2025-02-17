import { useEffect, useState } from "react";
import Loading from "../components/Loading";
import MovieCard from "@/components/movie";

type Movie = {
  id: string;
  title: string;
  rating: string;
  price: number;
  poster: string;
};

const Home = () => {
  const [movies, setMovies] = useState<Movie[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [retrying, setRetrying] = useState<boolean>(false);
  const [hasMounted, setHasMounted] = useState<boolean>(false);

  const fetchMovies = async () => {
    setLoading(true);
    setError(null); 

    try {
      const url = `${process.env.NEXT_PUBLIC_API_URL}/Movies`;
      const res = await fetch(url);

      if (!res.ok) {
        setError(`Failed to fetch movies. Status: ${res.status}`);
        return;
      }

      const data = await res.json();
      setMovies(data);
    } catch (error: any) {
      setError("API is unavailable. Please try again later.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    setHasMounted(true);
  }, []);

  const handleRetry = () => {
    setError(null); 
    setRetrying(!retrying); 
  };

  useEffect(() => {
    if (hasMounted) {
      fetchMovies();
    }
  }, [retrying, hasMounted]);

  return (
    <div className="container bg-background">
      <h1 className="text-4xl font-bold text-primary text-center my-8">Movies</h1>

      {loading ? (
        <Loading /> 
      ) : error ? (
        <div className="text-center">
          <p className="text-xl text-red-500 mb-4">{error}</p> 
          <button
            className="bg-primary text-white px-6 py-2 rounded-lg hover:bg-primary-dark transition duration-300"
            onClick={handleRetry}
          >
            Retry
          </button>
        </div>
      ) : movies.length === 0 ? (
        <p className="text-center text-xl text-red-900">No movies available at the moment.</p> // If no movies exist
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
          {movies.map((movie) => (
            <MovieCard key={movie.id} movie={movie} /> 
          ))}
        </div>
      )}
    </div>
  );
};

export default Home;
