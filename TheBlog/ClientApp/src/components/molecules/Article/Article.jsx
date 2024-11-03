import { useState } from "react";
import { Button, ButtonGroup } from "reactstrap";
import CommentSection from "../../organisms/CommentSection/CommentSection";
import "./Article.scss";

export default function Article({
  id,
  title,
  text,
  author,
  datetime,
  image,
  onEdit,
  onDelete,
  onLoadComments,
}) {
  const [isRemoved, setIsRemoved] = useState(false);

  const handleDelete = () => {
    // setIsRemoved(true);
    onDelete(() => setIsRemoved(true));
  };

  return (
    <article className="article border rounded-2 mt-3">
      {isRemoved ? (
        <h1 className="m-2 fs-6 fw-lighter">Removed</h1>
      ) : (
        <div className="pt-1">
          <div className="article__header m-2 mt-0 d-flex justify-content-between">
            <h2 className="article__author fs-6 fw-lighter">
              Posted by {author} ({datetime})
            </h2>
            <div className="position-relative">
              <ButtonGroup vertical className="position-absolute top-0 end-0">
                <Button outline size="sm" color="secondary" onClick={onEdit}>
                  <i className="bi bi-pencil-square" />
                </Button>
                <Button outline size="sm" color="danger" onClick={handleDelete}>
                  <i className="bi bi-trash-fill" />
                </Button>
              </ButtonGroup>
            </div>
          </div>
          <h1 className="article__title fs-4 mx-2">{title}</h1>
          <p className="article__text fs-6 mx-2">{text}</p>
          {image && (
            <img
              className="img-fluid"
              style={{ maxHeight: "90vh" }}
              src={image.href}
              alt={image.alt}
              loading="lazy"
            />
          )}
          <CommentSection articleId={id} />
        </div>
      )}
    </article>
  );
}
