import { useEffect, useState } from "react";
import { Button, Form, Input, InputGroup } from "reactstrap";
import ErrorList from "../../atoms/ErrorList/ErrorList";
import InteractiveComment from "../../molecules/InteractiveComment/InteractiveComment";

export default function CommentSection({ articleId }) {
  const [show, setShow] = useState(false);
  const [comments, setComments] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [isFetchingComments, setIsFetchingComments] = useState(false);
  const [inputValue, setInputValue] = useState("");
  const [errors, setErrors] = useState([]);

  const handleToggle = (e) => {
    e.preventDefault();
    setShow(!show);
  };

  const fetchComments = async () => {
    await setIsFetchingComments(true);
    await fetch(`/api/article/${articleId}/comment/all`).then(async (data) => {
      if (data.ok) {
        const comments = await data.json();
        setComments(comments);
      } else {
        setErrors([await data.text()]);
      }
    });
    setIsFetchingComments(false);
  };

  useEffect(() => {
    if (show && !isFetchingComments) {
      fetchComments();
    }
  }, [show, isLoading]);

  const handlePostComment = async (event) => {
    event.preventDefault();
    setIsLoading(true);

    const formData = new FormData(event.target);
    formData.append("model.ArticleId", articleId);

    await fetch(event.target.action, {
      method: "POST",
      body: formData,
    }).then(async (data) => {
      if (data.ok) {
        setInputValue("");
        setErrors([]);
      } else {
        setErrors([await data.text()]);
      }
    });
    setIsLoading(false);
  };

  const handleInputChange = (event) => {
    event.preventDefault();
    setInputValue(event.target.value);
  };

  const convertDate = (dateString) => {
    return new Date(dateString).toLocaleDateString("default", {
      year: "numeric",
      month: "2-digit",
      day: "2-digit",
      hour: "2-digit",
      minute: "2-digit",
      hour12: false,
    });
  };

  const handleUpdateComment = async (event, id) => {
    event.preventDefault();
    const formData = new FormData(event.target);
    formData.append("model.CommentId", id);

    await fetch(`/api/article/${articleId}/comment`, {
      method: "PATCH",
      body: formData,
    }).then(async (data) => {
      if (data.ok) {
        setErrors([]);
      } else {
        setErrors([await data.text()]);
      }
    });

    fetchComments();
  };

  const handleDeleteComment = async (id) => {
    await fetch(`/api/article/${articleId}/comment?commentId=${id}`, {
      method: "DELETE",
    }).then(async (data) => {
      if (data.ok) {
        setErrors([]);
      } else {
        setErrors([await data.text()]);
      }
    });

    fetchComments();
  };

  const getCommentsView = () => {
    return (
      <div className="mt-2">
        {comments.map((comment) => (
          <InteractiveComment
            key={comment.id}
            author={comment.author}
            datetime={convertDate(comment.created)}
            text={comment.text}
            onUpdate={(e) => {
              handleUpdateComment(e, comment.id);
            }}
            onDelete={() => {
              handleDeleteComment(comment.id);
            }}
          />
        ))}
        <Form
          onSubmit={handlePostComment}
          action={`/api/article/${articleId}/comment`}
          autoComplete="off"
        >
          <InputGroup className="mt-3">
            <Input
              id="inputText1"
              name="model.Text"
              type="text"
              placeholder="Send a comment"
              required
              onChange={handleInputChange}
              value={inputValue}
            />
            <Button type="submit" disabled={isLoading}>
              <i className="bi bi-send" />
            </Button>
          </InputGroup>
        </Form>
        <ErrorList errors={errors} />
      </div>
    );
  };

  return (
    <section className="border p-1">
      <a href="." onClick={handleToggle}>
        {show ? "Hide" : "Show"} comments
      </a>
      {show && getCommentsView()}
    </section>
  );
}
