import { useEffect, useState } from "react";
import Loading from "../components/Loading";

type Movie = {
  id: string;
  title: string;
  rating: string;
  price: number;
};

const Home = () => {
  const [movies, setMovies] = useState<Movie[]>([]);
  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    const fetchMovies = async () => {
      try {
        const res = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/Movies`);
        const data = await res.json();
        setMovies(data);
      } catch (error) {
        console.error("Failed to fetch movies:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchMovies();
  }, []);

  return (
    <div className="container bg-background">
      <h1 className="text-4xl font-bold text-primary text-center my-8">Movies</h1>

        {loading ? (
        <Loading />
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
          {movies.map((movie) => (
            <div
              key={movie.id}
              className="bg-white p-6 rounded-lg shadow-md hover:shadow-xl transition-shadow bg-gradient-to-r from-red-50 via-red-100 to-red-200"
            >
              <h2 className="text-2xl font-semibold text-primary">{movie.title}</h2>
              <p className="text-lg text-gray-700">Rating: {movie.rating}</p>
              <p className="text-lg text-gray-700">Price: ${movie.price.toFixed(2)}</p>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default Home;
