import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import Article from "../../molecules/Article/Article";

export default function Articles() {
  const navigate = useNavigate();
  const [articles, setArticles] = useState([]);

  const fetchArticles = async () => {
    await fetch('/api/article/getall')
    .then(async (data) => {
      if (data.ok) {
        const jsonData = await data.json();
        setArticles(jsonData);
      }
    })
  }

  const convertDate = (dateString) => {
    return new Date(dateString).toLocaleDateString('default', {
      year: 'numeric',
      month: '2-digit',
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit',
      hour12: false,
    });
  };

  useEffect(() => {
    fetchArticles();
  }, [])

  const handleArticleEdit = (id) => {
    navigate(`/article/edit?id=${id}`);
  };

  const handleArticleDelete = async (id, onSuccessCallback) => {
    await fetch(`/api/article/delete?id=${id}`, {
      method: "DELETE",
    })
    .then((res) => {
      if (!res.ok) {
        alert('Failed to delete article!');
      } else {
        onSuccessCallback();
      }
    });
  };

  return (
    <section className="pb-3">
      {articles.map((article) => (
        <Article
          key={article.id}
          id={article.id}
          author={article.authorName}
          datetime={convertDate(article.created)}
          title={article.title}
          text={article.text}
          image={article.image}
          onEdit={() => handleArticleEdit(article.id)}
          onDelete={(onSuccessCallback) => handleArticleDelete(article.id, onSuccessCallback)}
        />
      ))}
    </section>
  );
}
