export default function Comment({ author, datetime, text }) {
  return (
    <div className="border border-round m-1 w-100">
      <div className="m-2 mt-0">
        <h2 className="fs-6 mx-1 text-secondary fw-lighter">
          {author} ({datetime})
        </h2>
      </div>
      <p className="fs-6 mx-2">{text}</p>
    </div>
  );
}
