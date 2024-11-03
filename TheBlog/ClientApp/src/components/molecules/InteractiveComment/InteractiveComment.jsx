import { useState } from "react";
import { Button } from "reactstrap";
import Comment from "../../atoms/Comment/Comment";
import EditComment from "../../atoms/EditComment/EditComment";

export default function InteractiveComment({
  author,
  datetime,
  text,
  onUpdate,
  onDelete,
}) {
  const [showEditButton, setShowEditButton] = useState(false);
  const [edit, setEdit] = useState(false);
  const [commentText, setCommentText] = useState(text);

  const handleEditClick = (e) => {
    setEdit(true);
  };

  const handleCancelClick = (e) => {
    setEdit(false);
    setShowEditButton(false);
  };

  const handleSubmit = async (e) => {
    onUpdate(e);
    setEdit(false);
    setCommentText(new FormData(e.target).get('model.Text'));
  };

  return (
    <div
      onMouseEnter={() => {
        setShowEditButton(true);
      }}
      onMouseLeave={() => {
        setShowEditButton(false);
      }}
    >
      {edit ? (
        <EditComment
          author={author}
          datetime={datetime}
          text={commentText}
          onUpdate={() => {}}
          onCancel={handleCancelClick}
          onSubmit={handleSubmit}
          onDelete={onDelete}
        />
      ) : (
        <div className="d-flex justify-content-between">
          <Comment author={author} datetime={datetime} text={commentText} />
          <div className="position-relative">
            {showEditButton && (
              <Button
                className="position-absolute top-0 end-0"
                outline
                size="sm"
                color="secondary"
                onClick={handleEditClick}
              >
                <i className="bi bi-pencil-square" />
              </Button>
            )}
          </div>
        </div>
      )}
    </div>
  );
}
