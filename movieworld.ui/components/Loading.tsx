import { motion } from "framer-motion";

const Loading = () => {
  return (
    <div className="flex flex-col justify-center items-center h-screen space-y-4">
      <motion.div 
        animate={{ rotate: 360 }} 
        transition={{ repeat: Infinity, duration: 1.5, ease: "linear" }}
      >
        <svg
          className="w-12 h-12 text-primary"
          viewBox="0 0 50 50"
          fill="none"
          xmlns="http://www.w3.org/2000/svg"
        >
          <circle
            className="opacity-25"
            cx="25"
            cy="25"
            r="20"
            stroke="currentColor"
            strokeWidth="5"
          />
          <motion.circle
            className="opacity-75"
            cx="25"
            cy="25"
            r="20"
            stroke="currentColor"
            strokeWidth="5"
            strokeDasharray="100"
            strokeDashoffset="100"
            animate={{
              strokeDashoffset: [100, 0],
            }}
            transition={{
              repeat: Infinity,
              duration: 1.5,
              ease: "easeInOut",
            }}
          />
        </svg>
      </motion.div>

      
      <motion.p 
        className="text-lg text-primary text-center"
        initial={{ opacity: 0 }}
        animate={{ opacity: 1 }}
        transition={{ duration: 1.2, repeat: Infinity, repeatType: "reverse" }}
      >
        Hang tight! We’re fetching the best movie deals for you... Please wait.
      </motion.p>
    </div>
  );
};

export default Loading;
