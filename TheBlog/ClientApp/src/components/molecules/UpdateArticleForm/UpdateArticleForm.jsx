import { useState } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import { Form, Input } from "reactstrap";
import ActionButton from "../../atoms/ActionButton/ActionButton";
import ErrorList from "../../atoms/ErrorList/ErrorList";
import ImageSelectInput from "../../atoms/ImageSelectInput/ImageSelectInput";

export default function UpdateArticleForm() {
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(false);
  const [errors, setErrors] = useState([]);
  const [searchParams, setSearchParams] = useSearchParams();

  const handleFormSubmit = async (e) => {
    e.preventDefault();
    setIsLoading(true);

    const formData = new FormData(e.target);
    formData.append('model.Id', searchParams.get('id'));

    await fetch(e.target.action, {
      method: "PUT",
      body: formData,
    })
    .then(async (res) => {
      if (res.ok) {
        setTimeout(() => {
          setTimeout(() => navigate('/'), 500);
        });
      } else {
        setErrors([`${await res.text()}`]);
        setIsLoading(false);
      }
    });
  };

  return (
    <Form
      className="article border rounded-2 mt-3"
      action="/api/article/update"
      onSubmit={handleFormSubmit}
      encType="multipart/form-data"
      autoComplete="off"
    >
      <div className="pt-2 mx-3 my-4">
        <h1 className="fs-2">Update article</h1>
        <hr/>
        <Input
          id="title1"
          name="model.Title"
          placeholder="Title"
          type="text"
          className="rounded-1"
          bsSize="sm"
        />
        <Input
          id="text1"
          name="model.Text"
          placeholder="Text (optional)"
          type="textarea"
          className="rounded-1 mt-3"
          bsSize="sm"
        />
        <ImageSelectInput
          id="imageFile1"
          name="model.Image"
        />
        <ErrorList errors={errors} />
        <ActionButton text="Update" loading={isLoading} args={{type: 'submit'}} />
      </div>
    </Form>
  );
}
