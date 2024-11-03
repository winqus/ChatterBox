export default function ErrorList ({ errors }) {
  return (
    <ul>
      {errors.map((error, index) =>
        <li key={index} className="text-danger">{error}</li>
      )}
    </ul>
  );
};
